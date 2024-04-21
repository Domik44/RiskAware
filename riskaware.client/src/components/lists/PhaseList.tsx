import { useEffect, useMemo, useState } from 'react';
import {
  MaterialReactTable, useMaterialReactTable,
  type MRT_ColumnDef, type MRT_ColumnFiltersState,
  type MRT_PaginationState, type MRT_SortingState
} from 'material-react-table';
import { Box, Tooltip, IconButton } from '@mui/material';
import { ColumnSort } from '@tanstack/react-table';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import MUITableCommonOptions from '../../common/MUITableCommonOptions';
import { formatDate } from '../../helpers/DateFormatter';
import IDtParams from '../interfaces/IDtParams';
import IDtResult from '../interfaces/DtResult';
import IPhases from '../interfaces/IPhases';
import IFetchData from '../../common/IFetchData';
import IProjectDetail, { RoleType } from '../interfaces/IProjectDetail';
import PhaseDeleteModal from '../modals/PhaseDeleteModal';
import PhaseEditModal from '../modals/PhaseEditModal';

export const PhaseList: React.FC<{
  projectId: number,
  fetchDataRef: React.MutableRefObject<IFetchData | null>,
  reRender: () => void,
  projectDetail: IProjectDetail;
}> = ({ projectId, fetchDataRef, reRender, projectDetail }) => {
  // Data and fetching state
  const [data, setData] = useState<IPhases[]>([]);
  const [isError, setIsError] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [isRefetching, setIsRefetching] = useState(false);
  const [rowCount, setRowCount] = useState(0);

  // Table state
  const initialColumnSort: ColumnSort = { id: 'order', desc: false };
  const [columnFilters, setColumnFilters] = useState<MRT_ColumnFiltersState>([]);
  const [globalFilter, setGlobalFilter] = useState('');
  const [sorting, setSorting] = useState<MRT_SortingState>([initialColumnSort]);
  const [pagination, setPagination] = useState<MRT_PaginationState>({
    pageIndex: 0,
    pageSize: 10,
  });

  // Modals
  const [selectedPhaseId, setSelectedPhaseId] = useState<number | null>(null);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [editModalOpen, setEditModalOpen] = useState(false);
  const [editData, setEditData] = useState<IPhases>();

  const openDeleteModal = (phaseId: number) => {
    setSelectedPhaseId(phaseId);
    setDeleteModalOpen(true);
  };

  const toggleDeleteModal = () => {
    setDeleteModalOpen(!deleteModalOpen);
  };

  const fetchEditData = async (phaseId: number) => {
    try {
      const response = await fetch(`/api/ProjectPhase/${phaseId}`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json'
        }
      });
      const json: IPhases = await response.json();
      setEditData(json);
      setEditModalOpen(true);
    }
    catch (error) {
      console.error(error);
    }
  }

  const openEditModal = (phaseId: number) => {
    setSelectedPhaseId(phaseId);
    fetchEditData(phaseId);
  }

  const toggleEditModal = () => {
    setEditModalOpen(!editModalOpen);
  };

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
      sorting: sorting.length != 0 ? sorting : [initialColumnSort],
    };
    try {
      const response = await fetch(`/api/RiskProject/${projectId}/Phases`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(searchParams)
      });
      const json: IDtResult<IPhases> = await response.json();
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

  const columns = useMemo<MRT_ColumnDef<IPhases>[]>(
    () => [
      {
        id: 'order',
        accessorKey: 'order',
        header: 'Pořadí',
        filterFn: 'startsWith',
      },
      {
        id: 'name',
        accessorKey: 'name',
        header: 'Název',
        filterFn: 'startsWith',
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
    ],
    []
  );

  const table = useMaterialReactTable({
    ...MUITableCommonOptions<IPhases>(), // Add common and basic options
    columns,
    data,
    onColumnFiltersChange: setColumnFilters,
    onGlobalFilterChange: setGlobalFilter,
    onPaginationChange: setPagination,
    onSortingChange: setSorting,
    rowCount,
    enableFilters: false,
    enableTopToolbar: false,
    state: {
      columnFilters,
      globalFilter,
      isLoading,
      pagination,
      showAlertBanner: isError,
      showProgressBars: isRefetching,
      sorting,
    },
    enableRowActions: projectDetail.userRole === RoleType.ProjectManager,        // Display row actions
    renderRowActions: ({ row }) => (
      <Box sx={{ display: 'flex', gap: '1rem' }}>
        <Tooltip title="Upravit">
        {/*TODO -> zmenit na edit*/}
          <IconButton onClick={() => openEditModal(row.original.id)}>
            <EditIcon />
          </IconButton>
        </Tooltip>
        <Tooltip title="Vymazat">
          <IconButton color="error" onClick={() => openDeleteModal(row.original.id)}>
            <DeleteIcon />
          </IconButton>
        </Tooltip>
      </Box>
    ),
  });

  return (
    <div>
      <MaterialReactTable table={table} />
      <PhaseDeleteModal phaseId={selectedPhaseId ?? 0} isOpen={deleteModalOpen} toggle={toggleDeleteModal} fetchDataRef={fetchDataRef} reRender={reRender} />
      <PhaseEditModal phaseId={selectedPhaseId ?? 0} isOpen={editModalOpen} toggle={toggleEditModal} data={editData} reRender={reRender} fetchDataRef={fetchDataRef} projectId={projectId} />
    </div>
  );
}

export default PhaseList;
