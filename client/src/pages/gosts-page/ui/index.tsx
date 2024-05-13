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
        <InfiniteScroll
          dataLength={count} //This is important field to render the next data
          next={fetchGostsData}
          hasMore={count > countFetched}
          loader={<h4>Loading...</h4>}
          endMessage={
            <p style={{ textAlign: 'center' }}>
              <b>Yay! You have seen it all</b>
            </p>
          }
        >
          <section className={styles.filterSection}>
            <Filter 
              filterSubmit={(filterData: gostModel.GostFields & {name? :string}) => setGostParams(filterData)}
            />
          </section>
          <section className={styles.gostSection}>
            <GostsTable gosts={gosts || []} />
          </section>
        </InfiniteScroll>
    </div>
  )
}

export default GostsPage;