import react from '@vitejs/plugin-react';
import svgr from 'vite-plugin-svgr';
import { defineConfig } from 'vitest/config';
export default defineConfig({
  plugins: [
    react(),
    svgr({
      include: '**/*.svg?react',
      svgrOptions: {
        exportType: 'named',
        ref: true,
        svgo: false,
        titleProp: true,
      },
    }),
  ],
  test: {
    css: false,
    environment: 'jsdom',
    exclude: ['**/build/**', '**/node_modules/**'],
    fakeTimers: { toFake: undefined },
    globals: true,
    outputFile: 'coverage/test-report.xml',
    pool: 'vmThreads',
    poolOptions: {
      vmThreads: {
        maxThreads: 4,
        memoryLimit: '1GB',
        minThreads: 2,
        useAtomics: true,
      },
    },
    reporters: 'verbose',
    setupFiles: 'vitest.setup.tsx',
  },
});
