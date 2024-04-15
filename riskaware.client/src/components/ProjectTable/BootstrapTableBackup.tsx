//import React, { useState, useEffect } from 'react';
//import BootstrapTable from 'react-bootstrap-table-next';
//import paginationFactory from 'react-bootstrap-table2-paginator';

//interface IProject {
//  id: number;
//  title: string;
//  start: Date;
//  end: Date;
//  numOfMembers: string;
//  projectManagerName: string;
//}

//const RemotePagination = ({ data, page, sizePerPage, onTableChange, totalSize }) => (
//  <div>
//    <BootstrapTable
//      remote
//      keyField="id"
//      data={data}
//      columns={columns}
//      pagination={paginationFactory({ page, sizePerPage, totalSize })}
//      onTableChange={onTableChange}
//    />
//    <Code>{sourceCode}</Code>
//  </div>
//);

//const ProjectTable: React.FC<{ projects: IProject[] }> = ({ projects }) => {
//  const [loading, setLoading] = useState(true);
//  const [data, setData] = useState<IProject[]>(projects);
//  const [currentPage, setCurrentPage] = useState(1);
//  //const [searchText, setSearchText] = useState('');
//  //const [sortField, setSortField] = useState('');
//  //const [sortOrder, setSortOrder] = useState<'asc' | 'desc'>('asc');

//  useEffect(() => {
//    setData(projects);
//    setLoading(false);
//  }, [projects]);

//  const handlePageChange = (page: number) => {
//    setCurrentPage(page);
//  };

//  const pageSize = 10;
//  const startIndex = (currentPage - 1) * pageSize;
//  const endIndex = startIndex + pageSize;
//  const currentData = data.slice(startIndex, endIndex);

//  const columns = [
//    { dataField: 'id', text: 'ID', sort: true },
//    { dataField: 'title', text: 'Title', sort: true },
//    { dataField: 'start', text: 'Start Date', sort: true },
//    { dataField: 'end', text: 'End Date', sort: true },
//    { dataField: 'numOfMembers', text: 'Number of Members', sort: true },
//    { dataField: 'projectManagerName', text: 'Project Manager', sort: true },
//  ];

//  if (loading) {
//    return <div>Loading...</div>;
//  }

//  return (
//    <>
//      <input
//        type="text"
//        placeholder="Search"
//        value={searchText}
//        onChange={handleSearch}
//      />
//      <BootstrapTable
//        keyField="id"
//        data={currentData}
//        columns={columns}
//        defaultSorted={[{ dataField: 'id', order: 'asc' }]}
//        striped
//        hover
//        noDataIndication="No projects found"
//        pagination={paginationFactory({
//          sizePerPage: pageSize,
//          totalSize: sortedData.length,
//          onPageChange: handlePageChange,
//        })}
//      />
//    </>
//  );
//};

//export default ProjectTable;
