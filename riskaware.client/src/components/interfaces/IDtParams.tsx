import { MRT_ColumnFiltersState, MRT_SortingState } from "material-react-table";

interface ISearchParams {
  start: number;
  size: number;
  filters: MRT_ColumnFiltersState;
  sorting: MRT_SortingState;
}

export default ISearchParams;
