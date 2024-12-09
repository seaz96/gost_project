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
}

const Filter: React.FC<FilterProps> = props => {
  const {
    filterSubmit,
  } = props
  const dropdownRef = useRef<HTMLDivElement>(null)
  const [filterData, setFilterData] = useState<Partial<gostModel.GostFields> & {text?: string}>({
    "text": '',
    "designation": '',
    "fullName": '',
    "codeOks": '',
    "activityField": '',
    "acceptanceYear": '',
    "commissionYear":  '',
    "author": '',
    "acceptedFirstTimeOrReplaced": '',
    "content": '',
    "keyWords": '',
    "applicationArea": '',
    "documentText": '',
    "changes": '',
    "amendments": '',
})


  const handleSubmit = () => {
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

  const [filterOpen, setFilterOpen] = useState(false)

  return (
    <div className={styles.filterContainer}>
      <input 
        type='text' 
        className={styles.input} 
        value={filterData.text}
        onChange={(event) => setFilterData({...filterData, text: event.target.value})}
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
        <IconButton onClick={() => handleSubmit()} isFilled className={styles.searchButton}>
          <img src={search} alt='search'/>
        </IconButton>
        <Collapse className={styles.filterDropdown} in={filterOpen}>
          <div ref={dropdownRef}>
            <FilterDropdown 
              filterData={filterData}
              filterSubmit={(filterData) => setFilterData(filterData)}
            />
          </div>
        </Collapse>
      </div>
    </div>
  )
}

export default Filter;