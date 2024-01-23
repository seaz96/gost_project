import classNames from 'classnames'
import React from 'react'
import { Link } from 'react-router-dom'

import styles from './Statistic.module.scss'

const StatisticPage = () => {
  return (
    <div className='container'>
        <section className={classNames(styles.statistic, 'contentContainer')}>
            <Link to='/reviews-statistic-page' className={classNames(styles.statisticButton, 'baseButton', 'filledButton')}>Создание отчета по количеству обращений</Link>
            <Link to='/changes-statistic-page' className={classNames(styles.statisticButton, 'baseButton', 'filledButton')}>Создание отчета по количеству стандартов</Link>
        </section>
    </div>
  )
}

export default StatisticPage