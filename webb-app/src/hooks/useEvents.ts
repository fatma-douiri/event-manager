import { useCallback, useState } from 'react';
import { useDispatch } from 'react-redux';
import { setGlobalFilters } from '../store/reducers/eventSlice';
import { useGetEventsQuery } from '../store/services/eventService';
import type { EventStatus } from '../types/eventTypes';

type TEventFilters = {
  location?: string;
  status?: EventStatus;
  upcomingOnly?: boolean;
  pageSize?: number;
};

export const useEvents = (initialFilters: TEventFilters = {}) => {
  const dispatch = useDispatch();
  const [pageNumber, setPageNumber] = useState(1);
  const [filters, setFilters] = useState<TEventFilters>(initialFilters);

  const {
    data: events,
    isLoading,
    error,
  } = useGetEventsQuery({
    ...filters,
    pageNumber,
    pageSize: filters.pageSize ?? 10,
  });

  const updateFilters = useCallback(
    (newFilters: Partial<TEventFilters>) => {
      const updated = { ...filters, ...newFilters };
      setFilters(updated);
      dispatch(setGlobalFilters(updated));
      setPageNumber(1);
    },
    [dispatch, filters],
  );

  const handlePageChange = useCallback((page: number) => {
    setPageNumber(page);
  }, []);

  return {
    events,
    isLoading,
    error,
    filters,
    pageNumber,
    updateFilters,
    handlePageChange,
  };
};
