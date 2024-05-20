import React from 'react'
import { Filter } from 'widgets/filter';
import { GostsTable } from 'widgets/gosts-table';

import styles from './ArchivePage.module.scss'
import { gostModel, useGostsWithPagination } from 'entities/gost';
import InfiniteScroll from 'react-infinite-scroll-component';
const ArchivePage = () => {
    const {gosts, countFetched, count, setGostParams, fetchGostsData } = useGostsWithPagination('/docs/all-canceled')

    return (
      <div className='container contentContainer'>
          <InfiniteScroll
              dataLength={countFetched}
              next={fetchGostsData}
              hasMore={count > countFetched}
              loader={<p style={{textAlign: 'center'}}>
                  <b>Загрузка...</b>
              </p>}
          >

              <section className={styles.filterSection}>
                  <Filter
                      filterSubmit={(filterData: gostModel.GostFields & { name?: string }) => setGostParams(filterData)}
                  />
              </section>
              <section className={styles.gostSection}>
                  <GostsTable gosts={gosts || []}/>
              </section>
              <section>
                  {count === 0 ? (
                      <p style={{textAlign: 'center'}}>
                          <b>Документов нет.</b>
                      </p>
                  ) : null}
              </section>
          </InfiniteScroll>
      </div>
    )
}

export default ArchivePage;