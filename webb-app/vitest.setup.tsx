// @ts-nocheck
import '@testing-library/jest-dom/vitest';
import { configure as testingLibraryConfigure } from '@testing-library/react';

const intersectionObserverMock = () => ({
  observe: vi.fn(),
  unobserve: vi.fn(),
  disconnect: vi.fn(),
});
window.IntersectionObserver = vi.fn().mockImplementation(intersectionObserverMock);
window.open = vi.fn();
window.scrollTo = vi.fn();

testingLibraryConfigure({ testIdAttribute: 'data-test' });
