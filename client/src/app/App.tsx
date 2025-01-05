import { withProviders } from './providers'
import AppRouter from 'pages'
import './styles/index.scss'

const App = () => {
    return (
        <AppRouter />
    )
}

export const AppWithProviders = withProviders(App);