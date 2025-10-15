import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig(({ mode }) => {
  
  return {
    plugins: [react()],
    server: {
      port: 3000,
      strictPort: true, 
      host: true
    },
    build: {
      sourcemap: true
    }
  }
})
