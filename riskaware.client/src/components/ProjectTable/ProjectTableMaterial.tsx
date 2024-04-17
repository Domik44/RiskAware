import {
  useEffect,
  useMemo,
  useState
} from 'react';
import {
  MaterialReactTable,
  useMaterialReactTable,
  type MRT_Row,
  type MRT_ColumnDef,
  type MRT_ColumnFiltersState,
  type MRT_PaginationState,
  type MRT_SortingState,
} from 'material-react-table';
import {
  Box,
  Button,
  Tooltip,
  IconButton,
  createTheme,
  ThemeProvider,
  useTheme,
} from '@mui/material';
import { ColumnSort } from '@tanstack/react-table';
import { formatDate } from '../../helpers/DateFormatter';

import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import VisibilityOutlinedIcon from '@mui/icons-material/VisibilityOutlined';

import { MRT_Localization_CS } from 'material-react-table/locales/cs';
import { csCZ } from '@mui/material/locale';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFnsV3';
import { cs } from 'date-fns/locale';


type RiskProjectApiResponse = {
  data: Array<RiskProject>;
  totalRowCount: number;
};

// todo make rather interface
type RiskProject = {
  id: number;
  title: string;
  start: Date;
  end: Date;
  numOfMembers: string;
  projectManagerName: string;
}

const ProjectTableMaterial: React.FC = () => {
  //data and fetching state
  const [data, setData] = useState<RiskProject[]>([]);
  const [isError, setIsError] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [isRefetching, setIsRefetching] = useState(false);
  const [rowCount, setRowCount] = useState(0);

  //table state
  const initialColumnSort: ColumnSort = { id: 'id', desc: true };
  const [columnFilters, setColumnFilters] = useState<MRT_ColumnFiltersState>([]);
  const [globalFilter, setGlobalFilter] = useState('');
  const [sorting, setSorting] = useState<MRT_SortingState>([initialColumnSort]);
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

      //const url = new URL('/api/RiskProjects2');
      //const startOffset = pagination.pageIndex * pagination.pageSize;
      //url.searchParams.set('start', `${startOffset}`);
      //url.searchParams.set('size', `${pagination.pageSize}`);
      //url.searchParams.set('filters', JSON.stringify(columnFilters ?? []));
      //url.searchParams.set('globalFilter', globalFilter ?? '');
      //url.searchParams.set('sorting', JSON.stringify(sorting ?? []));
      //console.log(url.searchParams);

      const startOffset = pagination.pageIndex * pagination.pageSize;
      let searchParams = {
        start: startOffset,
        size: pagination.pageSize,
        filters: columnFilters ?? [],
        sorting: sorting.length != 0 ? sorting : [initialColumnSort],
      };
      console.log(searchParams);
      try {
        const response = await fetch('/api/RiskProjects2', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(searchParams)
        });
        const json = (await response.json()) as RiskProjectApiResponse;
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
    columnFilters, //re-fetch when column filters change
    globalFilter, //re-fetch when global filter changes
    pagination.pageIndex, //re-fetch when page index changes
    pagination.pageSize, //re-fetch when page size changes
    sorting, //re-fetch when sorting changes
  ]);

  const columns = useMemo<MRT_ColumnDef<RiskProject>[]>(
    () => [
      {
        accessorKey: 'id',
        header: 'ID',
        filterFn: 'equalss',
      },
      {
        accessorKey: 'title',
        header: 'Název',
      },
      {
        accessorFn: (row) => new Date(row.start),
        id: 'start',
        header: 'Začátek',
        filterVariant: 'date',
        filterFn: 'lessThan',   // todo change to something else
        sortingFn: 'datetime',
        Cell: ({ cell }) => formatDate(cell.getValue<Date>()),
        dateSetting: { locale: "cs-CZ" }, // todo delete
      },
      {
        accessorFn: (row) => new Date(row.end),
        id: 'end',
        header: 'Konec',
        filterVariant: 'date',
        filterFn: 'lessThan',   // todo change to something else
        sortingFn: 'datetime',
        Cell: ({ cell }) => formatDate(cell.getValue<Date>()),
      },
      {
        accessorKey: 'numOfMembers',
        header: 'Počet členů',
      },
      {
        accessorKey: 'projectManagerName',
        header: 'Projektový manažer',
      },
    ],
    [],
  );

  // todo copy delete confirm modal from ITU
  //DELETE action
  const openDeleteConfirmModal = (row: MRT_Row<RiskProject>) => {
    if (window.confirm(`Opravdu chcete vymazat projekt č. ${row.original.id} - ${row.original.title}?`)) {
      console.log(`Delete:${row.original.id}`);  // todo post delete
    }
  };

  // todo move options to object and reuse
  const table = useMaterialReactTable({
    //renderCaption: () => `Tabulka projektů`,   // todo rendering below
    //renderTopToolbarCustomActions: () => (
    //  <Row>
    //    <Col>
    //      <h4>Všechny projekty - MT</h4>
    //    </Col>
    //    <Col>
    //      <CreateProjectModal>
    //      </CreateProjectModal>
    //    </Col>
    //  </Row>
    //),
    columns,
    data,
    enableRowSelection: false,
    enableColumnFilterModes: false,   // todo maybe set to true but restrict to only one filter mode
    getRowId: (row) => String(row.id),
    initialState: {
      showColumnFilters: true,
      showGlobalFilter: false,
      columnPinning: {
        right: ['mrt-row-actions'],
      },
      density: 'compact',
    },
    enableGlobalFilter: false,    // todo delete global filter or alternatively create fulltext index to flex in PIS (*nerd)
    enableRowActions: true,
    positionActionsColumn: 'last',
    manualFiltering: true,
    manualPagination: true,
    manualSorting: true,
    muiToolbarAlertBannerProps: isError
      ? {
        color: 'error',
        children: 'Chyba při načítání dat',
      }
      : undefined,
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
    localization: MRT_Localization_CS,
    paginationDisplayMode: 'pages',
    positionToolbarAlertBanner: 'bottom',
    muiSearchTextFieldProps: {
      size: 'small',
      variant: 'outlined',
    },
    muiPaginationProps: {
      color: 'secondary',
      rowsPerPageOptions: [5, 7, 10, 12, 15, 20, 25, 50],
      shape: 'rounded',
      variant: 'outlined',
    },
    renderRowActions: ({ row }) => (
      <Box sx={{ display: 'flex', gap: '1rem' }}>
        <Tooltip title="Zobrazit detail">
          <IconButton href={`/api/RiskProject/${row.original.id}/Detail`}>
            <VisibilityOutlinedIcon />
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
  });

  const theme = useTheme();   // todo maybe it should be on App.tsx
  return (
    <ThemeProvider theme={createTheme(theme, csCZ)}>
      <LocalizationProvider dateAdapter={AdapterDateFns} adapterLocale={cs}>
        <MaterialReactTable table={table} />
      </LocalizationProvider>
    </ThemeProvider>
  );
};

export default ProjectTableMaterial;
