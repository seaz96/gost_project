

import { withProviders } from './providers'
import AppRouter from 'pages'
import './styles/index.scss'

const App = () => {
  
  return (
    <>
      <AppRouter />
    </>
  )
}

export default withProviders(App);