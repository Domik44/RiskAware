import { useEffect, useMemo, useState, useContext } from 'react';
import {
  MaterialReactTable, useMaterialReactTable,
  type MRT_Row, type MRT_ColumnDef, type MRT_ColumnFiltersState,
  type MRT_PaginationState, type MRT_SortingState
} from 'material-react-table';
import { Box, Tooltip, IconButton } from '@mui/material';
import { ColumnSort } from '@tanstack/react-table';
import MUITableCommonOptions from '../../common/MUITableCommonOptions';
import { formatDate } from '../../common/DateFormatter';
import IDtFetchData from '../interfaces/IDtFetchData';
import IDtParams from '../interfaces/IDtParams';
import IDtResult from '../interfaces/IDtResult';
import IProject from '../interfaces/IProject';
import DetailIcon from '@mui/icons-material/VisibilityOutlined';
import DeleteIcon from '@mui/icons-material/Delete';
import RestoreIcon from '@mui/icons-material/Restore';
import AuthContext from '../../auth/AuthContext';
import { useNavigate } from 'react-router-dom';
import DeleteProjectModal from '../modals/DeleteProjectModal';

export const ProjectsList: React.FC<{
  fetchUrl: string,
  fetchDataRef: React.MutableRefObject<IDtFetchData | null>,
}> = ({ fetchUrl, fetchDataRef }) => {
  const authContext = useContext(AuthContext);
  const navigate = useNavigate();

  // Data and fetching state
  const [data, setData] = useState<IProject[]>([]);
  const [isError, setIsError] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [isRefetching, setIsRefetching] = useState(false);
  const [rowCount, setRowCount] = useState(0);

  // Table state
  const initialColumnSort: ColumnSort = { id: 'start', desc: true };
  const [columnFilters, setColumnFilters] = useState<MRT_ColumnFiltersState>([]);
  const [globalFilter, setGlobalFilter] = useState('');
  const [sorting, setSorting] = useState<MRT_SortingState>([initialColumnSort]);
  const [pagination, setPagination] = useState<MRT_PaginationState>({
    pageIndex: 0,
    pageSize: 10,
  });

  const fetchData = async () => {
    if (!data.length) {
      setIsLoading(true);
    }
    else {
      setIsRefetching(true);
    }

    const startOffset = pagination.pageIndex * pagination.pageSize;
    let searchParams: IDtParams = {
      start: startOffset,
      size: pagination.pageSize,
      filters: columnFilters ?? [],
      sorting: sorting ?? [],
    };
    try {
      const response = await fetch(fetchUrl, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(searchParams)
      });
      const json: IDtResult<IProject> = await response.json();
      setData(json.data);
      setRowCount(json.totalRowCount);
    }
    catch (error) {
      setIsError(true);
      console.error(error);
      return;
    }
    setIsError(false);
    setIsLoading(false);
    setIsRefetching(false);
  };
  fetchDataRef.current = fetchData;

  useEffect(() => {
    fetchDataRef.current?.();
  }, [
    columnFilters,
    globalFilter,
    pagination.pageIndex,
    pagination.pageSize,
    sorting,
  ]);

  const columns = useMemo<MRT_ColumnDef<IProject>[]>(
    () => [
      {
        id: 'title',
        accessorKey: 'title',
        header: 'Název',
      },
      {
        accessorFn: (row) => new Date(row.start),
        id: 'start',
        header: 'Začátek',
        filterVariant: 'date',
        filterFn: 'greaterThan',
        sortingFn: 'datetime',
        Cell: ({ cell }) => formatDate(cell.getValue<Date>()),
      },
      {
        accessorFn: (row) => new Date(row.end),
        id: 'end',
        header: 'Konec',
        filterVariant: 'date',
        filterFn: 'lessThan',
        sortingFn: 'datetime',
        Cell: ({ cell }) => formatDate(cell.getValue<Date>()),
      },
      {
        id: 'numOfMembers',
        accessorKey: 'numOfMembers',
        header: 'Počet členů',
      },
      {
        id: 'projectManagerName',
        accessorKey: 'projectManagerName',
        header: 'Projektový manažer',
      },
    ],
    []
  );

  // Modals
  const [selectedProjectId, setSelectedProjectId] = useState<number | null>(null);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);

  const openDeleteModal = (riskProjectId: number) => {
    setSelectedProjectId(riskProjectId);
    setDeleteModalOpen(true);
  };

  const toggleDeleteModal = () => {
    setDeleteModalOpen(!deleteModalOpen);
  };

  const restoreProject = async (row: MRT_Row<IProject>) => {
    const response = await fetch(`/api/RiskProject/${row.original.id}/RestoreProject`, {
      method: 'PUT',
    });
    if (!response.ok) {
      throw new Error(`Restore failed for projectId: ${row.original.id}`);
    }
    fetchDataRef.current?.();
  };

  const goTo = (projectId: number) => {
    navigate(`/project/${projectId}`);
  }

  const table = useMaterialReactTable({
    ...MUITableCommonOptions<IProject>(), // Add common and basic options
    columns,
    data,
    onColumnFiltersChange: setColumnFilters,
    onGlobalFilterChange: setGlobalFilter,
    onPaginationChange: setPagination,
    onSortingChange: setSorting,
    rowCount,
    state: {
      columnFilters,
      globalFilter,
      isLoading,
      pagination,
      showAlertBanner: isError,
      showProgressBars: isRefetching,
      sorting,
    },
    enableRowActions: true,        // Display row actions
    renderRowActions: ({ row }) => row.original.isValid ?
      (
        <Box sx={{ display: 'flex', gap: '1rem' }}>
          <Tooltip title="Zobrazit detail projektu">
            <IconButton onClick={() => goTo(row.original.id)}>
              <DetailIcon />
            </IconButton>
          </Tooltip>
          {authContext?.isAdmin && ( 
            <Tooltip title="Vymazat projekt">
              <IconButton color="error" onClick={() => openDeleteModal(row.original.id)}>
                <DeleteIcon />
              </IconButton>
            </Tooltip>
          )}
        </Box>
      )
      :
      (authContext?.isAdmin && (
        <Tooltip title="Obnovit projekt ze smazaných">
          <IconButton onClick={() => restoreProject(row)}>
            <RestoreIcon />
          </IconButton>
        </Tooltip>)
      ),
    muiTableBodyRowProps: (table) => ({
      className: table.row.original.isValid ? 'valid-item' : 'invalid-item',
    }),
  });

  return (
    <>
      <DeleteProjectModal riskProjectId={selectedProjectId ?? 0} toggle={toggleDeleteModal} fetchDataRef={fetchDataRef} isOpen={deleteModalOpen} />
      <MaterialReactTable table={table} />
    </>
  );
};

export default ProjectsList;
