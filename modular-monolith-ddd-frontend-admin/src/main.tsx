import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter } from 'react-router-dom';
import './index.css'
import App from './App.tsx'

const isDevelopment = import.meta.env.DEV;
createRoot(document.getElementById('root')!).render(    
  isDevelopment ? (
  <StrictMode>
      <BrowserRouter>
        <App />
      </BrowserRouter>
  </StrictMode>
) : (  
    <BrowserRouter>
      <App />
    </BrowserRouter> 
)
);