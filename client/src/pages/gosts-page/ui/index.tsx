import { Filter } from 'widgets/filter';
import { GostsTable } from 'widgets/gosts-table';

import styles from './GostsPage.module.scss'
import { gostModel } from 'entities/gost';
import { useGostsWithPagination } from 'entities/gost';
import InfiniteScroll from 'react-infinite-scroll-component';

const GostsPage = () => {
  const {gosts, countFetched, count, setGostParams, fetchGostsData } = useGostsWithPagination('/docs/all-valid')

  return (
    <div className='container contentContainer'>
        <section className={styles.filterSection}>
          <Filter 
              filterSubmit={(filterData: gostModel.GostFields & {name? :string}) => setGostParams(filterData)}
          />
        </section>
        <InfiniteScroll
          dataLength={countFetched}
          next={fetchGostsData}
          hasMore={count > countFetched}
          loader={<h4>Loading...</h4>}
          endMessage={
            <></>
          }
        >
          <section className={styles.gostSection}>
            <GostsTable gosts={gosts || []} />
          </section>
        </InfiniteScroll>
    </div>
  )
}

export default GostsPage;