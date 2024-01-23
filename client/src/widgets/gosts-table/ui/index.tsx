import React, { useState } from 'react'

import styles from './GostTable.module.scss'
import loop  from '../assets/loop.png';
import { gostModel } from 'entities/gost';
import eye from 'shared/assets/eye.svg';
import pen from 'shared/assets/pen.svg';
import { Link } from 'react-router-dom';
import classNames from 'classnames';
import { Collapse, Popover } from '@mui/material';

interface GostsTableProps {
  gosts: gostModel.Gost[],
  filterValue?: string
}

const GostsTable: React.FC<GostsTableProps> = props => {
  const {gosts, filterValue} = props

  return (
    <table className={styles.table}>
        <thead>
          <tr className={styles.tableRow}>
            <th>№</th>
            <th>Код ОКС</th>
            <th>Обозначение</th>
            <th>Наименование</th>
            <th>Действия</th>
          </tr>
        </thead>
        {filterValue 
          ? 
          gosts.map(gost => 
            (gost.primary.designation.includes(filterValue) || gost.primary.fullName.includes(filterValue)) && 
            <GostRow gost={gost} />
          ) 
          : 
          gosts.map(gost => <GostRow gost={gost} />)
        }   
    </table>
  )
}

interface GostRowProps {
  gost: gostModel.Gost
}

const GostRow:React.FC<GostRowProps> = ({gost}) => {
  const [dropdownOpen, setDropdownOpen] = useState(false)
  console.log(!!gost.actual)

  return (
    <tbody>
      <img 
        id={gost.docId.toString()}
        src={loop} 
        className={styles.loop}
        onClick={() => setDropdownOpen(!dropdownOpen)}
        alt='открыть сводку по сфере применения'
      />
      <Popover
        id={gost.docId.toString()}
        open={dropdownOpen}
        onClose={() => setDropdownOpen(false)}
      >
        <div className={styles.applicationAreaContainer}>
          {gost.primary.applicationArea}
        </div>
      </Popover>
      <tr>
        <td>{gost.docId}</td>
        <td>{gost.primary.codeOKS}</td>
        <td>{gost.primary.designation}</td>
        <td className={styles.gostDescription}>{gost.primary.fullName}</td>
        <td>
          <div className={styles.buttons}>
            <Link to={`/gost-review/${gost.docId}`} className={classNames(styles.tableButton, 'baseButton', 'coloredText')}>
              <img src={eye} alt='eye' className={styles.buttonIcon}/>
              Просмотр
            </Link>
            <Link to={`/gost-edit/${gost.docId}`} className={classNames(styles.tableButton, 'baseButton', 'filledButton')}>
              <img src={pen} alt='pen' className={styles.buttonIcon}/>
              Редактирование
            </Link>
          </div>
        </td>
      </tr>
    </tbody>
  )
}

export default GostsTable; 