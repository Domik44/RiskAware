//import BootstrapTable from 'react-bootstrap-table-next';
//import paginationFactory from 'react-bootstrap-table2-paginator';

//const ProjectTable = (props : any) => {
//  const { data, columns, currentPage, totalSize, handleTableChange } = props;
//  return (
//    <BootstrapTable
//      keyField="id"
//      remote
//      data={data}
//      columns={columns}
//      striped
//      noDataIndication="Žádný projekt nenalezen"
//      pagination={paginationFactory({
//        page: currentPage,
//        sizePerPage: 10,
//        totalSize,
//      })}
//      onTableChange={handleTableChange}
//    />
//  )
//};

//export default ProjectTable;

////interface IProject {
////  id: number;
////  title: string;
////  start: Date;
////  end: Date;
////  numOfMembers: string;
////  projectManagerName: string;
////}

////const { data } = useQuery(['/api/RiskProjects', pageNumber], asyncFunction, {
////  keepPreviousData: true,
////});


////  const [loading, setLoading] = useState(true);
////  const [data, setData] = useState<IProject[]>(projects);
////  const [currentPage, setCurrentPage] = useState(1);

////  // todoa add:
////  //const [searchText, setSearchText] = useState('');
////  //const [sortField, setSortField] = useState('');
////  //const [sortOrder, setSortOrder] = useState<'asc' | 'desc'>('asc');

////  useEffect(() => {
////    setData(projects);
////    setLoading(false);
////  }, [projects]);

////  const handlePageChange = (page: number) => {
////    setCurrentPage(page);
////  };

////  const pageSize = 10;
////  const startIndex = (currentPage - 1) * pageSize;
////  const endIndex = startIndex + pageSize;
////  const currentData = data.slice(startIndex, endIndex);

////  // todo add sort: true
////  // todo add formatter
////  const columns = [
////    {
////      dataField: 'id',
////      text: 'ID',
////    },
////    {
////      dataField: 'title',
////      text: 'Název',
////    },
////    {
////      dataField: 'start',
////      text: 'Začátek',
////    },
////    {
////      dataField: 'end',
////      text: 'Konec',
////    },
////    {
////      dataField: 'numOfMembers',
////      text: 'Počet členů',
////    },
////    {
////      dataField: 'projectManagerName',
////      text: 'Projektový manažer',
////    },
////  ];

////  if (loading) {
////    return <div>Loading...</div>;
////  }

////  return (
////    <>
////      <BootstrapTable
////        keyField="id"
////        data={currentData}
////        columns={columns}
////        defaultSorted={[{ dataField: 'id', order: 'asc' }]}
////        striped
////        hover
////        noDataIndication="No projects found"
////        pagination={paginationFactory({
////          sizePerPage: pageSize,
////          totalSize: sortedData.length,
////          onPageChange: handlePageChange,
////        })}
////      />
////    </>
////  );
////};

////export default ProjectTable;
