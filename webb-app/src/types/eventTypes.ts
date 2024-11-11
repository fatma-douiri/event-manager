import { TUser } from './userTypes';

export enum EventStatus {
  Active = 0,
  Cancelled = 1,
  Completed = 2,
}

export type TEvent = {
  id: number;
  title: string;
  description?: string;
  location: string;
  startDate: Date; // dd/MM/yyyy
  startTime: string; // HH:mm
  endDate: Date;
  endTime: string;
  maxCapacity: number;
  currentParticipants: number;
  organizerName: string;
  status: EventStatus;
  participants: TUser[];
};

export type TGetEventsParams = {
  location?: string;
  status?: EventStatus;
  upcomingOnly?: boolean;
  pageNumber?: number;
  pageSize?: number;
};

export interface ICreateEventRequest {
  title: string;
  description?: string;
  location: string;
  startDate: Date;
  startTime: string;
  endDate: Date;
  endTime: string;
  maxCapacity: number;
}

export interface IUpdateEventRequest extends ICreateEventRequest {
  id: number;
  status: EventStatus;
}
