import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { EventStatus } from '../../types/eventTypes';

interface EventFilters {
  location?: string;
  status?: EventStatus;
  upcomingOnly: boolean;
  pageNumber: number;
  pageSize: number;
}

interface EventState {
  filters: EventFilters;
}

const initialState: EventState = {
  filters: {
    upcomingOnly: false,
    pageNumber: 1,
    pageSize: 10,
  },
};

const eventSlice = createSlice({
  name: 'events',
  initialState,
  reducers: {
    setGlobalFilters: (state, action: PayloadAction<Partial<EventFilters>>) => {
      state.filters = { ...state.filters, ...action.payload };
    },
    resetGlobalFilters: (state) => {
      state.filters = initialState.filters;
    },
  },
});

export const { setGlobalFilters, resetGlobalFilters } = eventSlice.actions;
export default eventSlice.reducer;
