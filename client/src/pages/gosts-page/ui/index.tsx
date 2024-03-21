import { Filter } from 'widgets/filter';
import { GostsTable } from 'widgets/gosts-table';

import styles from './GostsPage.module.scss'
import { useAxios } from 'shared/hooks';
import { gostModel } from 'entities/gost';
import { useState } from 'react';

const GostsPage = () => {
  const [inputValue, setInputValue] = useState('')
  const {response, error, loading, setParams} = useAxios<gostModel.Gost[]>(
    'localhost:8080/api/docs/all-valid',
  )


  return (
    <div className='container contentContainer'>
        <section className={styles.filterSection}>
            <Filter 
              filterSubmit={(filterData: gostModel.GostFields) => setParams(filterData)}
              inputSubmit={(inputData: string) => setInputValue(inputData)}
            />
        </section>
        <section className={styles.gostSection}>
            <GostsTable gosts={response || []} filterValue={inputValue}/>
        </section>
    </div>
  )
}

export default GostsPage;