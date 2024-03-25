import React from 'react'
import { Filter } from 'widgets/filter';
import { GostsTable } from 'widgets/gosts-table';

import styles from './ArchivePage.module.scss'
import { gostModel, useGostsWithPagination } from 'entities/gost';
import { Pagination } from '@mui/material';

const ArchivePage = () => {
    const {activeGosts, page, count, changePage, setGostParams } = useGostsWithPagination('/docs/all-canceled')

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

export default ArchivePage;