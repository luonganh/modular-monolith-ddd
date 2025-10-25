import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { Provider } from "react-redux";
import store from "./shared/store/index.ts";
import './index.css'
import App from './App.tsx'

const isDevelopment = import.meta.env.DEV;
createRoot(document.getElementById('root')!).render(    
  isDevelopment ? (
  <StrictMode>  
    <Provider store={store}>
        <App />     
    </Provider>   
  </StrictMode>
) : (      
      <Provider store={store}>
        <App />     
    </Provider>   
)
);