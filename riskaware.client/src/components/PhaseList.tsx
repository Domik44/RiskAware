import { useEffect, useMemo, useState } from 'react';
import {
  MaterialReactTable, useMaterialReactTable,
  type MRT_Row, type MRT_ColumnDef, type MRT_ColumnFiltersState,
  type MRT_PaginationState, type MRT_SortingState
} from 'material-react-table';
import { Box, Tooltip, IconButton } from '@mui/material';
import { ColumnSort } from '@tanstack/react-table';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import MUITableCommonOptions from './../common/MUITableCommonOptions';
import { formatDate } from './../helpers/DateFormatter';
import IDtParams from './interfaces/IDtParams';
import IDtResult from './interfaces/DtResult';
import IPhases from './interfaces/IPhases';
import IFetchData from '../common/IFetchData';

export const PhaseList: React.FC<{
  projectId: number,
  fetchDataRef: React.MutableRefObject<IFetchData | null>,
}> = ({ projectId, fetchDataRef }) => {
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

  // todo copy delete confirm modal from ITU
  const openDeleteConfirmModal = (row: MRT_Row<IPhases>) => {
    if (window.confirm(`Opravdu chcete vymazat projekt č. ${row.original.id} - ${row.original.name}?`)) {
      console.log(`Delete:${row.original.id}`); // todo post delete
    }
  };

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
    enableRowActions: true,        // Display row actions
    renderRowActions: ({ row }) => (
      <Box sx={{ display: 'flex', gap: '1rem' }}>
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
}

export default PhaseList;
