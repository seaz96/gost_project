import { Filter } from 'widgets/filter';
import { GostsTable } from 'widgets/gosts-table';

import styles from './GostsPage.module.scss'
import { useAxios } from 'shared/hooks';
import { gostModel } from 'entities/gost';
import { useState } from 'react';
import { Pagination } from '@mui/material';
import { useGostsWithPagination } from 'entities/gost';

const GostsPage = () => {
  const {activeGosts, page, count, changePage, setGostParams } = useGostsWithPagination('/docs/all-valid')

  return (
    <div className='container contentContainer'>
        <section className={styles.filterSection}>
          <Filter 
            filterSubmit={(filterData: gostModel.GostFields & {name? :string}) => setGostParams(filterData)}
          />
        </section>
        <section className={styles.gostSection}>
          <GostsTable gosts={activeGosts || []} />
        </section>
        <Pagination
          count={count}
          className={styles.gostsPagination}
          page={page}
          onChange={(event, value) => changePage(value)}
        />
    </div>
  )
}

export default GostsPage;