import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'
import viteTsconfigPaths from 'vite-tsconfig-paths'
import path from 'path'
import { VitePWA } from 'vite-plugin-pwa'

// https://vite.dev/config/
export default defineConfig({
    build: {
        outDir: 'build',
        sourcemap: true
    }, 
    plugins: [
      react(),
      viteTsconfigPaths(),
      VitePWA({
        registerType: 'autoUpdate',
        devOptions: {
          enabled: true,
        },
      })
    ],
    resolve: {
      alias: {
        'components': path.resolve(__dirname, './src/components'),
        'layout': path.resolve(__dirname, './src/layout'),
        'pages': path.resolve(__dirname, './src/pages'),
        'store': path.resolve(__dirname, './src/store'),
        'utils': path.resolve(__dirname, './src/utils'),
        'configuration': path.resolve(__dirname, './src/configuration'),
        'assets': path.resolve(__dirname, './src/assets'),
        'images': path.resolve(__dirname, './src/assets/images'),
        'types': path.resolve(__dirname, './src/types'),
      }
    },
    server: {
      port: 3000,
      strictPort: true, 
      host: true
    }    
})
