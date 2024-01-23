import { gostModel } from 'entities/gost'
import React, { useState } from 'react'

import styles from './FilterDropdown.module.scss';
import arrowDown from '../../assets/arrowDown.png';
import { Button, Input, RadioGroup } from 'shared/components';
import { Collapse } from '@mui/material';
import classNames from 'classnames';
import { useAxios } from 'shared/hooks';

interface FilterDropdownProps {
    filterSubmit: Function
}

const FilterDropdown: React.FC<FilterDropdownProps> = props => {
    const {
        filterSubmit
    } = props
    const [filterData, setFilterData] = useState<Partial<gostModel.GostFields>>({
        "designation": '',
        "fullName": '',
        "codeOKS": '',
        "activityField": '',
        "acceptanceDate": '',
        "commissionDate":  '',
        "author": '',
        "acceptedFirstTimeOrReplaced": '',
        "content": '',
        "keyWords": '',
        "keyPhrases": '',
        "applicationArea": '',
        "documentText": '',
        "changes": '',
        "amendments": '',
    })

    const [filterStatus, setFilterStatus] = useState({
        "designation": '',
        "fullName": '',
        "codeOKS": false,
        "activityField": false,
        "acceptanceDate": false,
        "commissionDate":  false,
        "author": false,
        "acceptedFirstTimeOrReplaced": false,
        "content": false,
        "keyWords": false,
        "keyPhrases": false, 
        "applicationArea": false,
        "adoptionLevel": false,
        "documentText": false,
        "changes": false,
        "amendments": false,
        "status": false,
        "harmonization": false,
        "isPrimary": true,
        "referencesId": false
    })

  return (
    <div className={styles.dropdown}>
        <div className={styles.dropdownItem}>
            <div className={styles.dropdownItemInfo} onClick={() => setFilterStatus({...filterStatus, codeOKS: !filterStatus.codeOKS})}>
                <img src={arrowDown} className={classNames(styles.arrowDown, filterStatus.codeOKS ? styles.arrowUp : '')}/>
                <p className={styles.dropdownItemName}>Код ОКС</p>
            </div>
            <Collapse className={styles.dropdownItemFilter} in={filterStatus.codeOKS}>
                <Input 
                    type='text' 
                    value={filterData.codeOKS} 
                    onChange={(value: string) => setFilterData({...filterData, codeOKS: value})} 
                />
            </Collapse>
        </div>
        <div className={styles.dropdownItem}>
            <div className={styles.dropdownItemInfo} onClick={() => setFilterStatus({...filterStatus, activityField: !filterStatus.activityField})}>
                <img src={arrowDown} className={classNames(styles.arrowDown, filterStatus.activityField ? styles.arrowUp : '')}/>
                <p className={styles.dropdownItemName}>Сфера деятельности</p>
            </div>
            <Collapse className={styles.dropdownItemFilter} in={filterStatus.activityField}>
                <Input 
                    type='text' 
                    value={filterData.activityField} 
                    onChange={(value: string) => setFilterData({...filterData, activityField: value})} 
                />
            </Collapse>
        </div>
        <div className={styles.dropdownItem}>
            <div className={styles.dropdownItemInfo} onClick={() => setFilterStatus({...filterStatus, acceptanceDate: !filterStatus.acceptanceDate})}> 
                <img src={arrowDown} className={classNames(styles.arrowDown, filterStatus.acceptanceDate ? styles.arrowUp : '')}/>
                <p className={styles.dropdownItemName}>Год принятия</p>
            </div>
            <Collapse className={styles.dropdownItemFilter} in={filterStatus.acceptanceDate}>
                <Input 
                    type='date' 
                    value={filterData.acceptanceDate} 
                    onChange={(value: string) => setFilterData({...filterData, acceptanceDate: value})} 
                />
            </Collapse>
        </div>
        <div className={styles.dropdownItem}>
            <div className={styles.dropdownItemInfo} onClick={() => setFilterStatus({...filterStatus, commissionDate: !filterStatus.commissionDate})}>
                <img src={arrowDown} className={classNames(styles.arrowDown, filterStatus.commissionDate ? styles.arrowUp : '')}/>
                <p className={styles.dropdownItemName}>Год введения</p>
            </div>
            <Collapse className={styles.dropdownItemFilter} in={filterStatus.commissionDate}>
                <Input 
                    type='date' 
                    value={filterData.commissionDate} 
                    onChange={(value: string) => setFilterData({...filterData, commissionDate: value})} 
                />
            </Collapse>
        </div>
        <div className={styles.dropdownItem}>
            <div className={styles.dropdownItemInfo} onClick={() => setFilterStatus({...filterStatus, author: !filterStatus.author})}>
                <img src={arrowDown} className={classNames(styles.arrowDown, filterStatus.author ? styles.arrowUp : '')}/>
                <p className={styles.dropdownItemName}>Разработчик</p>
            </div>
            <Collapse className={styles.dropdownItemFilter} in={filterStatus.author}>
                <Input 
                    type='text' 
                    value={filterData.author} 
                    onChange={(value: string) => setFilterData({...filterData, author: value})} 
                />
            </Collapse>
        </div>
        <div className={styles.dropdownItem}>
            <div className={styles.dropdownItemInfo} onClick={() => setFilterStatus({...filterStatus, acceptedFirstTimeOrReplaced: !filterStatus.acceptedFirstTimeOrReplaced})}>
                <img src={arrowDown} className={classNames(styles.arrowDown, filterStatus.acceptedFirstTimeOrReplaced ? styles.arrowUp : '')}/>
                <p className={styles.dropdownItemName}>Принят впервые/взамен</p>
            </div>
            <Collapse className={styles.dropdownItemFilter} in={filterStatus.acceptedFirstTimeOrReplaced}>
                <Input 
                    type='text' 
                    value={filterData.acceptedFirstTimeOrReplaced} 
                    onChange={(value: string) => setFilterData({...filterData, acceptedFirstTimeOrReplaced: value})} 
                />
            </Collapse>
        </div>
        <div className={styles.dropdownItem}>
            <div className={styles.dropdownItemInfo} onClick={() => setFilterStatus({...filterStatus, content: !filterStatus.content})}>
                <img src={arrowDown} className={classNames(styles.arrowDown, filterStatus.content ? styles.arrowUp : '')}/>
                <p className={styles.dropdownItemName}>Содержание</p>
            </div>
            <Collapse className={styles.dropdownItemFilter} in={filterStatus.content}>
                <Input 
                    type='text' 
                    value={filterData.content} 
                    onChange={(value: string) => setFilterData({...filterData, content: value})} 
                />
            </Collapse>
        </div>
        <div className={styles.dropdownItem}>
            <div className={styles.dropdownItemInfo} onClick={() => setFilterStatus({...filterStatus, applicationArea: !filterStatus.applicationArea})}>
                <img src={arrowDown} className={classNames(styles.arrowDown, filterStatus.applicationArea ? styles.arrowUp : '')}/>
                <p className={styles.dropdownItemName}>Область применения</p>
            </div>
            <Collapse className={styles.dropdownItemFilter} in={filterStatus.applicationArea}>
                <Input 
                    type='text' 
                    value={filterData.applicationArea} 
                    onChange={(value: string) => setFilterData({...filterData, applicationArea: value})} 
                />
            </Collapse>
        </div>
        <div className={styles.dropdownItem}>
            <div className={styles.dropdownItemInfo} onClick={() => setFilterStatus({...filterStatus, keyWords: !filterStatus.keyWords})}>
                <img src={arrowDown} className={classNames(styles.arrowDown, filterStatus.keyWords ? styles.arrowUp : '')}/>
                <p className={styles.dropdownItemName}>Ключевые слова</p>
            </div>
            <Collapse className={styles.dropdownItemFilter} in={filterStatus.keyWords}>
                <Input 
                    type='text' 
                    value={filterData.keyWords} 
                    onChange={(value: string) => setFilterData({...filterData, keyWords: value})} 
                />
            </Collapse>
        </div>
        <div className={styles.dropdownItem}>
            <div className={styles.dropdownItemInfo} onClick={() => setFilterStatus({...filterStatus, keyPhrases: !filterStatus.keyPhrases})}>
                <img src={arrowDown} className={classNames(styles.arrowDown, filterStatus.keyPhrases ? styles.arrowUp : '')}/>
                <p className={styles.dropdownItemName}>Ключевые фразы</p>
            </div>
            <Collapse className={styles.dropdownItemFilter} in={filterStatus.keyPhrases}>
                <Input 
                    type='text' 
                    value={filterData.keyPhrases} 
                    onChange={(value: string) => setFilterData({...filterData, keyPhrases: value})} 
                />
            </Collapse>
        </div>
        <div className={styles.dropdownItem}>
            <div className={styles.dropdownItemInfo} onClick={() => setFilterStatus({...filterStatus, adoptionLevel: !filterStatus.adoptionLevel})}>
                <img src={arrowDown} className={classNames(styles.arrowDown, filterStatus.adoptionLevel ? styles.arrowUp : '')}/>
                <p className={styles.dropdownItemName}>Уровень принятия</p>
            </div>
            <Collapse className={styles.dropdownItemFilter} in={filterStatus.adoptionLevel}>
                <RadioGroup 
                  buttons={[
                    {id:'International', value:'0', label:'Международный'},
                    {id:'Foreign', value:'1', label:'Иностранный'},
                    {id:'Regional', value:'2', label:'Региональный'},
                    {id:'Organizational', value:'3', label:'Организационный'},
                    {id:'National', value:'4', label:'Национальный'},
                    {id:'Interstate', value:'5', label:'Межгосударственный'},
                  ]} 
                  direction='vertical'
                  name='adoptionLevel'
                  value={filterData.adoptionLevel?.toString() || '0'}
                  onChange={(value: string) => {setFilterData({...filterData, adoptionLevel: parseInt(value)})}}
                />
            </Collapse>
        </div>
        <div className={styles.dropdownItem}>
            <div className={styles.dropdownItemInfo} onClick={() => setFilterStatus({...filterStatus, documentText: !filterStatus.documentText})}>
                <img src={arrowDown} className={classNames(styles.arrowDown, filterStatus.documentText ? styles.arrowUp : '')}/>
                <p className={styles.dropdownItemName}>Текст стандарта</p>
            </div>
            <Collapse className={styles.dropdownItemFilter} in={filterStatus.documentText}>
                <Input 
                    type='text' 
                    value={filterData.documentText} 
                    onChange={(value: string) => setFilterData({...filterData, documentText: value})} 
                />
            </Collapse>
        </div>
        <div className={styles.dropdownItem}>
            <div className={styles.dropdownItemInfo} onClick={() => setFilterStatus({...filterStatus, changes: !filterStatus.changes})}>
                <img src={arrowDown} className={classNames(styles.arrowDown, filterStatus.changes ? styles.arrowUp : '')}/>
                <p className={styles.dropdownItemName}>Изменения</p>
            </div>
            <Collapse className={styles.dropdownItemFilter} in={filterStatus.changes}>
                <Input 
                    type='text' 
                    value={filterData.changes} 
                    onChange={(value: string) => setFilterData({...filterData, changes: value})} 
                />
            </Collapse>
        </div>
        <div className={styles.dropdownItem}>
            <div className={styles.dropdownItemInfo} onClick={() => setFilterStatus({...filterStatus, amendments: !filterStatus.amendments})}>
                <img src={arrowDown} className={classNames(styles.arrowDown, filterStatus.amendments ? styles.arrowUp : '')}/>
                <p className={styles.dropdownItemName}>Поправки</p>
            </div>
            <Collapse className={styles.dropdownItemFilter} in={filterStatus.amendments}>
                <Input 
                    type='text' 
                    value={filterData.amendments} 
                    onChange={(value: string) => setFilterData({...filterData, amendments: value})} 
                />
            </Collapse>
        </div>
        <div className={styles.dropdownItem}>
            <div className={styles.dropdownItemInfo} onClick={() => setFilterStatus({...filterStatus, harmonization: !filterStatus.harmonization})}>
                <img src={arrowDown} className={classNames(styles.arrowDown, filterStatus.harmonization ? styles.arrowUp : '')}/>
                <p className={styles.dropdownItemName}>Уровень гармонизации</p>
            </div>
            <Collapse className={styles.dropdownItemFilter} in={filterStatus.harmonization}>
                <RadioGroup 
                  buttons={[
                    {id:'unharmonized', value:'0', label:'Негармонизированный'},
                    {id:'harmonized', value:'1', label:'Гармонизорованный'},
                    {id:'modified', value:'2', label:'Модифицированный'},
                  ]} 
                  name='harmonization'
                  direction='vertical'
                  value={filterData.harmonization?.toString() || '1'}
                  onChange={(value: string) => {setFilterData({...filterData, harmonization: parseInt(value)})}}
                />
            </Collapse>
        </div>
        <Button onClick={() => filterSubmit(filterData)} isFilled className={styles.submitButton}>Найти</Button>
    </div>
  )
}

export default FilterDropdown