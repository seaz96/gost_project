import React, { useContext, useState } from 'react'

import styles from './GostTable.module.scss'
import loop  from '../assets/loop.png';
import { gostModel } from 'entities/gost';
import eye from 'shared/assets/eye.svg';
import pen from 'shared/assets/pen.svg';
import { Link } from 'react-router-dom';
import classNames from 'classnames';
import { IconButton, Popover } from '@mui/material';
import { UserContext } from 'entities/user';

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
          gosts.map((gost, index) =>
            (gost.primary.designation.includes(filterValue) || gost.primary.fullName.includes(filterValue)) &&
              <GostRow gost={gost} number={index + 1} />
          ) 
          :
            gosts.map((gost, index) => <GostRow gost={gost} number={index + 1} />)
        }   
    </table>
  )
}

interface GostRowProps {
  gost: gostModel.Gost,
  number: number
}

const GostRow:React.FC<GostRowProps> = ({gost, number}) => {
  const [anchorEl, setAnchorEl] = React.useState<HTMLButtonElement | null>(null);
  const open = Boolean(anchorEl);
  const {user} = useContext(UserContext)
  const currentGostData = gost.primary || gost.actual

  if(currentGostData.designation)
    return (
      <tbody>
        <IconButton  
          id={gost.docId.toString()}
          className={styles.loop}
          onClick={(event) => {
            setAnchorEl(event.currentTarget);
          }}
        >
          <img src={loop} alt='открыть сводку по сфере применения'/>
        </IconButton>
        <Popover
          id={gost.docId.toString()}
          open={open}
          onClose={() => setAnchorEl(null)}
          anchorEl={anchorEl}
          anchorOrigin={{
            vertical: 'bottom',
            horizontal: 'center',
          }}
          sx={{
            '.css-3bmhjh-MuiPaper-root-MuiPopover-paper': {

            }
          }}
        >
          <div className={styles.applicationAreaContainer} style={{whiteSpace: "pre-line"}}>
            {currentGostData.applicationArea}
          </div>
        </Popover>
        <tr>
          <td>{number}</td>
          <td>{currentGostData.codeOKS}</td>
          <td>{currentGostData.designation}</td>
          <td className={styles.gostDescription}>{currentGostData.fullName}</td>
          <td>
            <div className={styles.buttons}>
              <Link to={`/gost-review/${gost.docId}`}
                    className={classNames(styles.tableButton, 'baseButton', 'coloredText')}>
                <img src={eye} alt='eye' className={styles.buttonIcon}/>
                Просмотр
              </Link>
              {
                (user?.role === 'Admin' || user?.role === 'Heisenberg') &&
                <Link to={`/gost-edit/${gost.docId}`}
                      className={classNames(styles.tableButton, 'baseButton', 'filledButton')}>
                  <img src={pen} alt='pen' className={styles.buttonIcon}/>
                  Редактирование
                </Link>
              }
            </div>
          </td>
        </tr>
      </tbody>
    )
  else return <></>
}

export default GostsTable; 