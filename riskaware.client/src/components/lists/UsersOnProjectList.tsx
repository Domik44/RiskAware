import { useEffect, useMemo, useState } from 'react';
import {
  MaterialReactTable, useMaterialReactTable,
  type MRT_Row, type MRT_ColumnDef, type MRT_ColumnFiltersState,
  type MRT_PaginationState, type MRT_SortingState
} from 'material-react-table';
import MUITableCommonOptions from '../../common/MUITableCommonOptions';
import { Box, Tooltip, IconButton } from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import IDtParams from '../interfaces/IDtParams';
import IDtResult from '../interfaces/IDtResult';
import IMembersList from '../interfaces/IMembersList';
import IDtFetchData from '../interfaces/IDtFetchData';


export const UsersOnProjectList: React.FC<{
  projectId: number,
  fetchDataRef: React.MutableRefObject<IDtFetchData | null>
}> = ({ projectId, fetchDataRef }) => {
  // Data and fetching state
  const [data, setData] = useState<IMembersList[]>([]);
  const [isError, setIsError] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [isRefetching, setIsRefetching] = useState(false);
  const [rowCount, setRowCount] = useState(0);

  // Table state
  const [columnFilters, setColumnFilters] = useState<MRT_ColumnFiltersState>([]);
  const [globalFilter, setGlobalFilter] = useState('');
  const [sorting, setSorting] = useState<MRT_SortingState>([]);
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
      const response = await fetch(`/api/RiskProject/${projectId}/Members`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(searchParams)
      });
      const json: IDtResult<IMembersList> = await response.json();
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

  const columns = useMemo<MRT_ColumnDef<IMembersList>[]>(
    () => [
      {
        id: 'fullName',
        accessorKey: 'fullname',
        header: 'Jméno',
        filterFn: 'startsWith',
      },
      {
        id: 'email',
        accessorKey: 'email',
        header: 'E-mail',
        filterFn: 'startsWith',
      },
      {
        id: 'roleName',
        accessorKey: 'roleName',
        header: 'Role',
        filterFn: 'startsWith',
      },
      {
        id: 'projectPhaseName',
        accessorKey: 'projectPhaseName',
        header: 'Fáze projektu',
        filterFn: 'startsWith',
      },
      {
        id: 'isReqApproved',
        accessorKey: 'isReqApproved',
        header: 'Schváleno přijetí',
        filterFn: 'startsWith',
        Cell: ({ cell }) => cell.getValue<boolean>() ? 'Schváleno' : 'Zamítnuto',
      },
    ],
    []
  );

  // todo copy delete confirm modal from ITU
  const openDeleteConfirmModal = (row: MRT_Row<IMembersList>) => {
    if (window.confirm(`Opravdu chcete vymazat projekt č. ${row.original.id} - ${row.original.fullName}?`)) {
      console.log(`Delete:${row.original.id}`); // todo post delete
    }
  };

  const table = useMaterialReactTable({
    ...MUITableCommonOptions<IMembersList>(), // Add common and basic options
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
    renderRowActions: ({ row }) => (
      <Box sx={{ display: 'flex', gap: '1rem' }}>
        {/*<Tooltip title="Zobrazit detail">*/}
        {/*  <IconButton href={`/xxx`}>*/}
        {/*    <DetailIcon />*/}
        {/*  </IconButton>*/}
        {/*</Tooltip>*/}
        <Tooltip title="Upravit">
          <IconButton onClick={() => openDeleteConfirmModal(row)}>
            <EditIcon />
          </IconButton>
        </Tooltip>
        <Tooltip title="Vymazat">
          <IconButton color="error" onClick={() => openDeleteConfirmModal(row)}>
            <DeleteIcon />
          </IconButton>
        </Tooltip>
      </Box>
    ),
  });

  return <MaterialReactTable table={table} />;
};

export default UsersOnProjectList;
