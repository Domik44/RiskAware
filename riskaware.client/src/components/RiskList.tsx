import { useEffect, useMemo, useState } from 'react';
import {
  MaterialReactTable, useMaterialReactTable,
  type MRT_Row, type MRT_ColumnDef, type MRT_ColumnFiltersState,
  type MRT_PaginationState, type MRT_SortingState
} from 'material-react-table';
import { Box, Button, Tooltip, IconButton } from '@mui/material';
import MUITableCommonOptions from './../common/MUITableCommonOptions';
import { formatDateForInput } from './../helpers/DateFormatter';
import IDtResult from './interfaces/DtResult';
import IRisks from './interfaces/IRisks';

import DetailIcon from '@mui/icons-material/VisibilityOutlined';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import FileDownloadIcon from '@mui/icons-material/FileDownload';

import { jsPDF } from 'jspdf';
import autoTable, { CellInput } from 'jspdf-autotable';
import { mkConfig, generateCsv, download, ColumnHeader } from 'export-to-csv';


export const RiskList: React.FC<{ projectId: number, chooseRisk: (id: number) => void }> = ({ projectId, chooseRisk }) => {
  // Data and fetching state
  const [data, setData] = useState<IRisks[]>([]);
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

  useEffect(() => {
    const fetchData = async () => {
      if (!data.length) {
        setIsLoading(true);
      }
      else {
        setIsRefetching(true);
      }

      const startOffset = pagination.pageIndex * pagination.pageSize;
      let searchParams = {
        start: startOffset,
        size: pagination.pageSize,
        filters: columnFilters ?? [],
        sorting: sorting ?? [],
      };
      try {
        const response = await fetch(`/api/RiskProject/${projectId}/Risks`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(searchParams)
        });
        const json: IDtResult<IRisks> = await response.json();
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
    fetchData();
  }, [
    columnFilters,
    globalFilter,
    pagination.pageIndex,
    pagination.pageSize,
    sorting,
  ]);

  const columns = useMemo<MRT_ColumnDef<IRisks>[]>(
    () => [
      {
        id: 'title',
        accessorKey: 'title',
        header: 'Název',
        filterFn: 'startsWith',
      },
      {
        id: 'categoryName',
        accessorKey: 'categoryName',
        header: 'Kategorie',
      },
      {
        id: 'severity',
        accessorKey: 'severity',
        header: 'Závažnost',
        filterFn: 'startsWith',
      },
      {
        id: 'probability',
        accessorKey: 'probability',
        header: 'Pravděpodobnost',
        filterFn: 'startsWith',
      },
      {
        id: 'impact',
        accessorKey: 'impact',
        header: 'Dopad',
        filterFn: 'startsWith',
      },
      {
        id: 'state',
        accessorKey: 'state',
        header: 'Stav',
      }
    ],
    []
  );

  // todo copy delete confirm modal from ITU
  const openDeleteConfirmModal = (row: MRT_Row<IRisks>) => {
    if (window.confirm(`Opravdu chcete vymazat projekt č. ${row.original.id} - ${row.original.title}?`)) {
      console.log(`Delete:${row.original.id}`); // todo post delete
    }
  };

  const exportToPDF = (rows: MRT_Row<IRisks>[]) => {
    const doc = new jsPDF();
    const tableData = rows.map((row) => Object.values(row.original) as CellInput[]);
    const tableHeaders = columns.map((c) => String(c.id));

    autoTable(doc, {
      head: [tableHeaders],
      body: tableData,
    });

    const now = new Date();
    const dateString = formatDateForInput(now);
    doc.save(`${dateString}_registr_rizik.pdf`);
  };

  const exportToCSV = () => {
    const tableHeaders: ColumnHeader[] = columns.map((col) => ({
      key: String(col.id),
      displayLabel: String(col.header)
    }));
    const formattedData = data.map(risk => ({
      title: risk.title,
      categoryName: risk.categoryName,
      severity: risk.severity,
      probability: risk.probability,
      impact: risk.impact,
      state: risk.state,
    }));

    const now = new Date();
    const dateString = formatDateForInput(now);
    const csvConfig = mkConfig({
      filename: `${dateString}_registr_rizik`,
      fieldSeparator: ',',
      decimalSeparator: '.',
      columnHeaders: tableHeaders,
    });
    const csv = generateCsv(csvConfig)(formattedData);
    download(csvConfig)(csv);
  };

  const table = useMaterialReactTable({
    ...MUITableCommonOptions<IRisks>(), // Add common and basic options
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
        <Tooltip title="Zobrazit detail">
          <IconButton onClick={() => chooseRisk(row.original.id)}>
            <DetailIcon />
          </IconButton>
        </Tooltip>
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
    muiToolbarAlertBannerProps: isError
      ? {
        color: 'error',
        children: 'Chyba při načítání dat',
      }
      : undefined,
    renderTopToolbarCustomActions: ({ table }) => (
      <Box>
        <Button
          disabled={table.getPrePaginationRowModel().rows.length === 0}
          onClick={() => exportToPDF(table.getPrePaginationRowModel().rows)}
          startIcon={<FileDownloadIcon />}
        >
          Exportovat do PDF
        </Button>
        <Button
          onClick={exportToCSV}
          startIcon={<FileDownloadIcon />}
        >
          Exportovat do CSV
        </Button>
      </Box>
    ),
  });

  return <MaterialReactTable table={table} />;
};

export default RiskList;
