import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import path from 'node:path'
import { fileURLToPath } from 'node:url'
import viteTsconfigPaths from 'vite-tsconfig-paths'

const __dirname = path.dirname(fileURLToPath(import.meta.url))

// https://vite.dev/config/
export default defineConfig({
  envDir: path.resolve(__dirname, '..'),
  envPrefix: ['VITE_', 'DASHBOARD_'],
  plugins: [
    react(),
    viteTsconfigPaths()
  ],
  server: {
      port: 3010,
      strictPort: true, 
      host: true
    } 
})
