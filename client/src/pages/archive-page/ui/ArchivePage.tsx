import React from 'react'
import { Filter } from 'widgets/filter';
import { GostsTable } from 'widgets/gosts-table';

import styles from './ArchivePage.module.scss'
import { useAxios } from 'shared/hooks';
import { gostModel } from 'entities/gost';

const ArchivePage = () => {
    const {response, loading, error} = useAxios<gostModel.Gost[]>('https://backend-seaz96.kexogg.ru/api/docs/all-canceled')

    if(loading) return <></>

    if(response)
        return (
            <div className='container'>
                <section className={styles.filterSection}>
                    <Filter />
                </section>
                <section className={styles.gostSection}>
                    <GostsTable gosts={response}/>
                </section>
            </div>
        )
    else return <></>
}

export default ArchivePage;