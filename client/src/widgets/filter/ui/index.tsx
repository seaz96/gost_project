import React, { useRef, useState } from 'react'

import styles from './Filter.module.scss'
import IconButton from 'shared/components/IconButton';

import filter from '../assets/filter.svg';
import search from '../assets/search.svg'
import FilterDropdown from './FilterDropdown';
import { gostModel } from 'entities/gost';
import { Collapse } from '@mui/material';

interface FilterProps {
  filterSubmit: Function
  inputSubmit: Function
}

const Filter: React.FC<FilterProps> = props => {
  const {
    filterSubmit,
    inputSubmit
  } = props
  const dropdownRef = useRef<HTMLDivElement>(null)

  const handleSubmit = (filterData: gostModel.GostFields) => {
    setFilterOpen(false);
    filterSubmit(filterData)
  }
 
  const handleFilterDropdownOpen = () => {
    const closeListener = (event: MouseEvent) => {
      if(event.target !== dropdownRef.current && !dropdownRef.current?.contains(event.target as HTMLElement)) {
        event.stopPropagation()
        setFilterOpen(false)
        document.removeEventListener('click', closeListener)
      }
    }
    setFilterOpen(true)
    document.addEventListener('click', (event) => closeListener(event))
  }

  const [inputValue, setInputValue] = useState('')
  const [filterOpen, setFilterOpen] = useState(false)

  return (
    <div className={styles.filterContainer}>
      <input 
        type='text' 
        className={styles.input} 
        value={inputValue} 
        onChange={(event) => setInputValue(event.target.value)}
        placeholder='Поиск по обозначению или наименованию...'
      />
      <div className={styles.buttonsContainer}>
        <IconButton 
          onClick={(event: React.MouseEvent) => {
            event.stopPropagation()
            filterOpen ? setFilterOpen(false) : handleFilterDropdownOpen()
          }} 
          isFilled 
          className={styles.filterButton}
        >
          <img src={filter} alt='filter'/>
        </IconButton>
        <IconButton onClick={() => inputSubmit(inputValue)} isFilled className={styles.searchButton}>
          <img src={search} alt='search'/>
        </IconButton>
        <Collapse className={styles.filterDropdown} in={filterOpen}>
          <div ref={dropdownRef}>
            <FilterDropdown filterSubmit={(filterData: gostModel.GostFields) => handleSubmit(filterData)}/>
          </div>
        </Collapse>
      </div>
    </div>
  )
}

export default Filter;