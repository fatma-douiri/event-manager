import react from '@vitejs/plugin-react';
import { defineConfig } from 'vite';
import { VitePWA, VitePWAOptions } from 'vite-plugin-pwa';
import svgr from 'vite-plugin-svgr';

const manifestForPlugIn: Partial<VitePWAOptions> = {
  manifest: {
    background_color: '#ffffff',
    description: 'Responsive React progressive web application that manage events.',
    display: 'standalone',
    icons: [
      {
        purpose: 'maskable',
        sizes: '16x16',
        src: '/favicon-16x16.png',
        type: 'image/png',
      },
      {
        sizes: '32x32',
        src: '/favicon-32x32.png',
        type: 'image/png',
      },
      {
        sizes: 'any',
        src: '/apple-touch-icon.png',
        type: 'image/png',
      },
      {
        sizes: '192x192',
        src: '/android-chrome-192x192.png',
        type: 'image/png',
      },
      {
        sizes: '512x512',
        src: '/android-chrome-512x512.png',
        type: 'image/png',
      },
    ],
    name: 'Event App',
    orientation: 'portrait',
    scope: '/',
    short_name: 'movie-app',
    start_url: '/',
    theme_color: '#000000',
  },
  registerType: 'prompt',
};

export default defineConfig({
  base: './',
  plugins: [
    svgr({
      include: '**/*.svg?react',
      svgrOptions: {
        exportType: 'named',
        ref: true,
        svgo: false,
        titleProp: true,
      },
    }),
    react(),
    VitePWA(manifestForPlugIn),
  ],
  server: {
    port: 5173,
    open: true,
    proxy: {
      '/api': {
        target: 'https://localhost:7231',
        changeOrigin: true,
        secure: false,
      },
    },
  },
});
