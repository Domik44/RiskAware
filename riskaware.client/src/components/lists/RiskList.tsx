import React, { useEffect, useMemo, useState } from 'react';
import ColumnFilter, {
  MaterialReactTable, useMaterialReactTable,
  type MRT_Row, type MRT_ColumnDef, type MRT_ColumnFiltersState,
  type MRT_PaginationState, type MRT_SortingState,
} from 'material-react-table';
import { Box, Button, Tooltip, IconButton } from '@mui/material';
import MUITableCommonOptions from '../../common/MUITableCommonOptions';
import { formatDateForInput } from '../../common/DateFormatter';
import IDtParams from '../interfaces/IDtParams';
import IDtResult from '../interfaces/IDtResult';
import IRisks from '../interfaces/IRisks';
import IDtFetchData from '../interfaces/IDtFetchData';
import RiskDeleteModal from '../modals/RiskDeleleModal';

import DetailIcon from '@mui/icons-material/VisibilityOutlined';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import FileDownloadIcon from '@mui/icons-material/FileDownload';

import { jsPDF } from 'jspdf';
import autoTable, { CellInput } from 'jspdf-autotable';
import { mkConfig, generateCsv, download, ColumnHeader } from 'export-to-csv';
import IRiskCategory from '../interfaces/IRiskCategory';
import IProjectDetail, { RoleType } from '../interfaces/IProjectDetail';
import RiskEditModal from '../modals/RiskEditModal';
import IRiskDetail from '../interfaces/IRiskDetail';
import { DatePicker } from '@mui/x-date-pickers';
import IRiskEdit from '../interfaces/IRiskEdit';
//import IRiskDetail from '../interfaces/IRiskDetail';

export const RiskList: React.FC<{
  projectId: number,
  chooseRisk: (id: number) => void,
  fetchDataRef: React.MutableRefObject<IDtFetchData | null>,
  reRender: () => void,
  projectDetail: IProjectDetail
}> = ({ projectId, chooseRisk, fetchDataRef, reRender, projectDetail }) => {
  // Data and fetching state
  const [data, setData] = useState<IRisks[]>([]);
  const [isError, setIsError] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [isRefetching, setIsRefetching] = useState(false);
  const [rowCount, setRowCount] = useState(0);
  const canEditWithoutPhase = projectDetail?.userRole === RoleType.ProjectManager || projectDetail?.userRole === RoleType.RiskManager;
  //const canEditWithPhase = projectDetail?.userRole === RoleType.TeamMember && projectDetail?.assignedPhase !== null;

  // Table state
  const [columnFilters, setColumnFilters] = useState<MRT_ColumnFiltersState>([]);
  const [globalFilter, setGlobalFilter] = useState('');
  const [sorting, setSorting] = useState<MRT_SortingState>([]);
  const [pagination, setPagination] = useState<MRT_PaginationState>({
    pageIndex: 0,
    pageSize: 10,
  });

  // Modals
  const [selectedRiskId, setSelectedRiskId] = useState<number | null>(null);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [editModalOpen, setEditModalOpen] = useState(false);
  const [editData, setEditData] = useState<IRiskEdit>();

  const [riskHistoryDate, setRiskHistoryDate] = useState<Date | null>(new Date());

  const openDeleteModal = (riskId: number) => {
    setSelectedRiskId(riskId);
    setDeleteModalOpen(true);
  };

  const toggleDeleteModal = () => {
    setDeleteModalOpen(!deleteModalOpen);
  };

  const fetchEditData = async (riskId: number) => {
    try {
      const response = await fetch(`/api/Risk/${riskId}/GetEdit`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json'
        }
      });
      const json: IRiskEdit = await response.json();
      setEditData(json);
      setEditModalOpen(true);
    }
    catch (error) {
      console.error(error);
    }
  }

  const openEditModal = (riskId: number) => {
    fetchRiskCategories();
    setSelectedRiskId(riskId);
    fetchEditData(riskId);
  }

  const toggleEditModal = () => {
    setEditModalOpen(!editModalOpen);
  };

  const [categoriesM, setCategoriesM] = useState<IRiskCategory[]>([]);
  const [categories, setCategories] = useState<string[]>([]);
  const fetchRiskCategories = async () => {
    let categoryNames: string[];
    try {
      const response = await fetch(`/api/RiskProject/${projectId}/RiskCategories`);
      if (!response.ok) {
        throw new Error('Něco se pokazilo! Zkuste to prosím znovu.');
      }
      else {
        const categories: IRiskCategory[] = await response.json();
        setCategoriesM(categories);
        categoryNames = categories.map(c => c.name);
        setCategories(categoryNames);
      }
    }
    catch (error: any) {
      console.error(error);
    }
  };

  const fetchData = async () => {
    if (!data.length) {
      setIsLoading(true);
    }
    else {
      setIsRefetching(true);
    }

    const riskHistoryDateValue = (document.querySelector('input[name="RiskHistoryDate"]') as HTMLInputElement).value;
    if (riskHistoryDateValue === "DD.MM.YYYY") {
      setRiskHistoryDate(new Date());   // Set to now
    }
    const startOffset = pagination.pageIndex * pagination.pageSize;
    const filters = columnFilters ?? [];
    const updatedFilters = [...filters, {
      id: "RiskHistoryDate",
      value: riskHistoryDate,
    }];

    let searchParams: IDtParams = {
      start: startOffset,
      size: pagination.pageSize,
      filters: updatedFilters,
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
  fetchDataRef.current = fetchData;

  useEffect(() => {
    fetchRiskCategories();
    fetchDataRef.current?.();
  }, [
    columnFilters,
    globalFilter,
    pagination.pageIndex,
    pagination.pageSize,
    sorting,
    riskHistoryDate,
  ]);

  const columns = useMemo<MRT_ColumnDef<IRisks>[]>(() => {
    return [
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
        filterVariant: 'select',
        filterSelectOptions: categories,
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
    ];
  }, [categories]);

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

  const tableInstance = useMaterialReactTable({
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
        {(row.original.projectPhase.id === projectDetail.assignedPhase?.id ||canEditWithoutPhase) && (
          <div>
            <Tooltip title="Upravit">
              <IconButton onClick={() => openEditModal(row.original.id)}>
                <EditIcon />
              </IconButton>
            </Tooltip>
            <Tooltip title="Vymazat">
              <IconButton color="error" onClick={() => openDeleteModal(row.original.id)}>
                <DeleteIcon />
              </IconButton>
            </Tooltip>
          </div>
        )}
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
          startIcon={<FileDownloadIcon />}>
          Exportovat do PDF
        </Button>
        <Button
          onClick={exportToCSV}
          startIcon={<FileDownloadIcon />}>
          Exportovat do CSV
        </Button>
      </Box>
    ),
  });

  return (
    <>
      <div id="RiskHistoryDateRow" className="input-group">
        <div className="input-group-prepend">
          <span className="input-group-text">Zobrazit rizika k datu:</span>
        </div>
        <DatePicker
          className="form-control"
          name="RiskHistoryDate"
          value={riskHistoryDate}
          onChange={(date) => setRiskHistoryDate(date)}
          minDate={new Date(projectDetail.detail.start)}
          disableFuture
          slotProps={{ field: { clearable: true } }} />
      </div>
      <MaterialReactTable table={tableInstance} />
      <RiskDeleteModal riskId={selectedRiskId ?? 0} isOpen={deleteModalOpen} toggle={toggleDeleteModal} reRender={reRender} fetchDataRef={fetchDataRef} />
      <RiskEditModal riskId={selectedRiskId ?? 0} isOpen={editModalOpen} toggle={toggleEditModal} data={editData} reRender={reRender} fetchDataRef={fetchDataRef} projectDetail={projectDetail} categories={categoriesM} />
    </>
  );
};

export default RiskList;
