import { useEffect } from 'react';
import { useQuery, useQueryClient } from 'react-query';
import { ITableChangeParams } from './ITableChangeParams';

const getData = async (tableParams: ITableChangeParams) => {
  const response = await fetch('/api/RiskProjects', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(tableParams)
  });
  const data = await response.json();
  return data;
};

export function useGetData(tableParams: ITableChangeParams) {
  const queryClient = useQueryClient();

  const { data, isLoading } = useQuery(
    ['unique-key', tableParams],
    () => getData(tableParams),
    {
      keepPreviousData: true,
      staleTime: 6000   // 60 seconds
    }
  );

  useEffect(() => {
    if (data) {
      tableParams.currentPage = tableParams.currentPage + 1;    // nextPage
      queryClient.prefetchQuery(
        ['unique-key', tableParams],
        () => getData(tableParams),
      );
    }
  }, [queryClient, tableParams, data]);

  return { data, isLoading };
}
