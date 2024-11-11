import { configureStore } from '@reduxjs/toolkit';
import authReducer from './reducers/authSlice';
import eventReducer from './reducers/eventSlice';
import { authApi } from './services/authService';
import { eventApi } from './services/eventService';

export const store = configureStore({
  devTools: import.meta.env.MODE !== 'production',
  reducer: {
    auth: authReducer,
    events: eventReducer,
    [authApi.reducerPath]: authApi.reducer,
    [eventApi.reducerPath]: eventApi.reducer,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(authApi.middleware, eventApi.middleware),
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
