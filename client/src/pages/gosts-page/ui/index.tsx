import React from 'react'
import { Filter } from 'widgets/filter';
import { GostsTable } from 'widgets/gosts-table';

import styles from './GostsPage.module.scss'
import { useAxios } from 'shared/hooks';
import { gostModel } from 'entities/gost';

const GostsPage = () => {
  const {response, error, loading} = useAxios<gostModel.Gost[]>('https://backend-seaz96.kexogg.ru/api/docs/all')

  return (
    <div className='container'>
        <section className={styles.filterSection}>
            <Filter />
        </section>
        <section className={styles.gostSection}>
            <GostsTable gosts={response || []}/>
        </section>
    </div>
  )
}

export default GostsPage;