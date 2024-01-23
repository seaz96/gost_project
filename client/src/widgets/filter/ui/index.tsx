import React from 'react'

import styles from './Filter.module.scss'
import IconButton from 'shared/components/IconButton';

import filter from '../assets/filter.svg';
import search from '../assets/search.svg'

const Filter = () => {
  return (
    <div className={styles.filterContainer}>
      <input type='text' className={styles.input}/>
      <IconButton onClick={() => {}} isFilled className={styles.filterButton}>
        <img src={filter} alt='filter'/>
      </IconButton>
      <IconButton onClick={() => {}} isFilled className={styles.searchButton}>
        <img src={search} alt='search'/>
      </IconButton>
    </div>
  )
}

export default Filter;