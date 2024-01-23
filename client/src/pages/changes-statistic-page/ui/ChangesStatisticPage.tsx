import React, { useState } from 'react'

import styles from './ChangesStatisticPage.module.scss'
import { ChangesStatisticForm } from 'widgets/changes-statistic-form';
import classNames from 'classnames';
import { gostModel } from 'entities/gost';
import { ChangesStatisticTable } from 'widgets/changes-statistic-table';

const ChangesStatisticPage = () => {
    const [changesData, setChangesData] = useState<gostModel.GostChanges | null>(null)
    const [startDate, setStartDate] = useState('')
    const [endDate, setEndDate] = useState('')
  

    return (
        <div className='container'>
            {changesData 
            ?
            <section className={classNames(styles.statistic, 'contentContainer')}>
              <h2 className={styles.title}>
                <span>Статистика</span> с {`${formatDate(new Date(startDate))}`} по {`${formatDate(new Date(endDate))}`}
              </h2>
              <ChangesStatisticTable changesData={changesData}/>
            </section>
            :
            <section className={classNames(styles.statistic, 'contentContainer')}>
              <ChangesStatisticForm 
                handleSubmit={(values:gostModel.GostChanges) => setChangesData(values)}
                startDateSubmit={setStartDate}
                endDateSubmit={setEndDate}
              />
            </section>
            }
        </div>
    )
}

const formatDate = (date: Date) => {
    let day = date.getDate().toString()
    day = day.length === 1 ? '0' + day : day;
    const month = date.getMonth() + 1
    const year = date.getFullYear()
    return `${day}.${month}.${year}`
}

export default ChangesStatisticPage;