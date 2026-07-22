import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import { BrowserRouter } from 'react-router-dom'

// BrowserRouter is a premade component that we can use to add rputing to our application
// It wraps the whole app so that any components can declare routes or link between them
// It uses the HTML5 history api(the thing that gives you browser history) - the URL changes
// but there's never a request for a new page. Its always that same index.html

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <BrowserRouter>
      <App />
    </BrowserRouter>
  </StrictMode>,
)
