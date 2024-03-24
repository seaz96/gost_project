import React, { useState } from 'react'
import { Filter } from 'widgets/filter';
import { GostsTable } from 'widgets/gosts-table';

import styles from './ArchivePage.module.scss'
import { useAxios } from 'shared/hooks';
import { gostModel, useGostsWithPagination } from 'entities/gost';
import { Pagination } from '@mui/material';

const ArchivePage = () => {
    const [filterParams, setFilterParams] = useState<gostModel.GostFields | null>(null)
    const {activeGosts, page, count, loading, changePage, setGostParams } = useGostsWithPagination('/docs/all-valid')


    if(loading) return <></>

    if(activeGosts)
        return (
            <div className='container contentContainer'>
                <section className={styles.filterSection}>
                    <Filter inputSubmit={() => {}} filterSubmit={(filterData: gostModel.GostFields) => setFilterParams(filterData)}/>
                </section>
                <section className={styles.gostSection}>
                    <GostsTable gosts={activeGosts}/>
                </section>
                <Pagination
                    count={count}
                    className={styles.gostsPagination}
                    page={page}
                    onChange={(event, value) => changePage(value)}
                />
            </div>
        )
    else return <></>
}

export default ArchivePage;