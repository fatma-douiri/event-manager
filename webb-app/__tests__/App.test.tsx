import { render } from '@testing-library/react';
import React from 'react';
import { Provider } from 'react-redux';
import { describe, expect, it, vi } from 'vitest';

import App from '../src/App';
import { store } from '../src/store';

vi.useFakeTimers();

describe('Renders main page correctly', async () => {
  it('Should render the page correctly', async () => {
    const container = render(
      <Provider store={store}>
        <App />
      </Provider>,
    );

    expect(container).toMatchSnapshot();
  });
});
