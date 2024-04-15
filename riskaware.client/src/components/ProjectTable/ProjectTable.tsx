import React, { useState, useEffect } from "react";
//import BootstrapTable, { ColumnDescription, TableChangeType } from 'react-bootstrap-table-next';
//import paginationFactory from 'react-bootstrap-table2-paginator';
import DataTable, { TableColumn } from 'react-data-table-component';
//import { useGetData } from "./customHook";
import { ITableChangeParams } from './ITableChangeParams';
import IProject from "../interfaces/IProject";

// todo add sort: true
// todo add formatter
const columns: TableColumn<IProject>[] = [
  {
    id: 'id',
    name: 'ID',
    selector: (row) => row.id,
    sortable: true,
  },
  {
    id: 'title',
    name: 'Název',
    selector: (row) => row.title,
    sortable: true,
  },
  {
    id: 'start',
    name: 'Začátek',
    selector: (row) => new Date(row.start).toLocaleDateString(),
    sortable: true,
  },
  {
    id: 'end',
    name: 'Konec',
    selector: (row) => new Date(row.end).toLocaleDateString(),
    sortable: true,
  },
  {
    id: 'numOfMembers',
    name: 'Počet členů',
    selector: (row) => row.numOfMembers,
    sortable: true,
  },
  {
    id: 'projectManagerName',
    name: 'Projektový manažer',
    selector: (row) => row.projectManagerName,
    sortable: true,
  },
];

const ProjectTable: React.FC = () => {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalRows, setTotalRows] = useState(0);
  const [perPage, setPerPage] = useState(10);
  const [sortField, setSortField] = useState('id');
  const [sortOrder, setSortOrder] = useState<'asc' | 'desc'>('desc');

  const fetchData = async (page: number, newPerPageSize = perPage, newSortField = sortField, newSortOrder = sortOrder) => {
    setLoading(true);

    const params: ITableChangeParams = {
      currentPage: page,
      perPage: newPerPageSize,
      sortField: newSortField,
      sortOrder: newSortOrder
    };

    //const { data, isLoading } = useGetData(params);

    const response = await fetch('/api/RiskProjects', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(params)
    });
    const data = await response.json();

    setData(data.data);
    setLoading(false);
    setTotalRows(data.totalsize);
  };

  useEffect(() => {
    fetchData(1);
  }, []);

  const handlePageChange = (currentPage: number) => {
    setCurrentPage(currentPage);
    fetchData(currentPage);
  };

  const handlePerRowsChange = async (newPerPage: number) => {
    setPerPage(newPerPage);
    fetchData(currentPage, newPerPage);
  };

  const handleSort = (column: TableColumn<IProject>, newSortOrder: 'asc' | 'desc') => {
    const newSortField = column.id as string;
    setSortField(newSortField);
    setSortOrder(newSortOrder);
    fetchData(currentPage, perPage, newSortField, newSortOrder);
  };

  return (
    <div className="table-responsive">
      <DataTable
        title="Všechny projekty"
        columns={columns}
        data={data}
        progressPending={loading}
        pagination
        paginationServer
        paginationTotalRows={totalRows}
        paginationDefaultPage={currentPage}
        onChangeRowsPerPage={handlePerRowsChange}
        onChangePage={handlePageChange}
        onSort={(column, sortOrder) => handleSort(column, sortOrder)}
        sortServer
        responsive
        className="table table-bordered table-striped"
      />
    </div>
  );
};

export default ProjectTable;
