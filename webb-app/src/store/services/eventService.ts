import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { TApiResponse } from '../../types/commonTypes';
import {
  ICreateEventRequest,
  IUpdateEventRequest,
  TEvent,
  TGetEventsParams,
} from '../../types/eventTypes';
import { TUser } from '../../types/userTypes';

export const eventApi = createApi({
  reducerPath: 'eventApi',
  baseQuery: fetchBaseQuery({
    baseUrl: '/api',
    prepareHeaders: (headers) => {
      const token = localStorage.getItem('token');
      if (token) {
        headers.set('authorization', `Bearer ${token}`);
      }
      return headers;
    },
  }),
  tagTypes: ['Event', 'EventParticipants'],
  endpoints: (builder) => ({
    // Get all events
    getEvents: builder.query<TApiResponse<TEvent[]>, TGetEventsParams>({
      query: (params) => ({
        url: '/event',
        method: 'GET',
        params: {
          location: params.location,
          status: params.status,
          upcomingOnly: params.upcomingOnly,
          pageNumber: params.pageNumber ?? 1,
          pageSize: params.pageSize ?? 10,
        },
      }),
      providesTags: (result) =>
        result
          ? [
              ...result.data.map(({ id }) => ({ type: 'Event' as const, id })),
              { type: 'Event', id: 'LIST' },
            ]
          : [{ type: 'Event', id: 'LIST' }],
    }),

    // Get a single event
    getEvent: builder.query<TApiResponse<TEvent>, number>({
      query: (id) => `/event/${id}`,
      providesTags: (_result, _error, id) => [{ type: 'Event', id }],
    }),

    // Create an event
    createEvent: builder.mutation<TApiResponse<number>, ICreateEventRequest>({
      query: (eventData) => ({
        url: '/event',
        method: 'POST',
        body: eventData,
      }),
      invalidatesTags: [{ type: 'Event', id: 'LIST' }],
    }),

    // Update an event
    updateEvent: builder.mutation<TApiResponse<void>, IUpdateEventRequest>({
      query: (eventData) => ({
        url: `/event/${eventData.id}`,
        method: 'PUT',
        body: eventData,
      }),
      invalidatesTags: (_result, _error, { id }) => [
        { type: 'Event', id },
        { type: 'Event', id: 'LIST' },
      ],
    }),

    // register to an event
    registerToEvent: builder.mutation<TApiResponse<void>, number>({
      query: (eventId) => ({
        url: `/event/${eventId}/register`,
        method: 'POST',
      }),
      invalidatesTags: (_result, _error, eventId) => [
        { type: 'Event', id: eventId },
        { type: 'Event', id: 'LIST' },
        { type: 'EventParticipants', id: eventId },
      ],
    }),

    // Unregister from an event
    unregisterFromEvent: builder.mutation<TApiResponse<void>, number>({
      query: (eventId) => ({
        url: `/event/${eventId}/register`,
        method: 'DELETE',
      }),
      invalidatesTags: (_result, _error, eventId) => [
        { type: 'Event', id: eventId },
        { type: 'Event', id: 'LIST' },
        { type: 'EventParticipants', id: eventId },
      ],
    }),

    // Get event participants
    getEventParticipants: builder.query<TApiResponse<TUser[]>, number>({
      query: (eventId) => `/event/events/${eventId}/participants`,
      providesTags: (_result, _error, eventId) => [{ type: 'EventParticipants', id: eventId }],
    }),
  }),
});

// Export des hooks
export const {
  useGetEventsQuery,
  useGetEventQuery,
  useCreateEventMutation,
  useUpdateEventMutation,
  useRegisterToEventMutation,
  useUnregisterFromEventMutation,
  useGetEventParticipantsQuery,
} = eventApi;
