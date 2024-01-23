import React from 'react'

import styles from './Loader.module.scss'
import { CircularProgress } from '@mui/material'

const Loader = () => {
  return (
    <div className={styles.loader}>
        <CircularProgress className={styles.loaderIcon} />
    </div>
  )
}

export default Loader