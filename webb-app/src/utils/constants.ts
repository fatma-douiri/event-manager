export const ROUTES = {
  HOME: '/',
  LOGIN: '/login',
  REGISTER: '/register',
  EVENTS: '/events',
  EVENT_DETAILS: '/events/:id',
  CREATE_EVENT: '/events/create',
} as const;

export const USER_ROLES = {
  ORGANIZER: 'Organizer',
  PARTICIPANT: 'Participant',
} as const;
