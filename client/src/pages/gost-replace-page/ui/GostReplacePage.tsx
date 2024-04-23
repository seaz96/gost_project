import axios from 'axios'
import classNames from 'classnames'
import React from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { GostForm, newGostModel } from 'widgets/gost-form'

import styles from './GostReplacePage.module.scss'
import { gostModel } from 'entities/gost'
import { useAxios } from 'shared/hooks'
import { axiosInstance } from 'shared/configs/axiosConfig'

function getGostStub() {
    return {
        "designation": '',
        "fullName": '',
        "codeOKS": '',
        "activityField": '',
        "acceptanceYear": '',
        "commissionYear":  '',
        "author": '',
        "acceptedFirstTimeOrReplaced": '',
        "content": '',
        "keyWords": '',
        "applicationArea": '',
        "adoptionLevel": 0,
        "documentText": '',
        "changes": '',
        "amendments": '',
        "status": 0,
        "harmonization": 1,
        "isPrimary": true,
        "references": []
    } as newGostModel.GostToSave
}

const GostReplacePage = () => {
    const navigate = useNavigate()
    const gostToReplaceId = useParams().id
    const {response, loading} = useAxios<gostModel.Gost>('/docs/' + gostToReplaceId)


    const addNewDocument = (gost: newGostModel.GostToSave) => {
      axiosInstance.post('/docs/add', gost)
      .then(() => {
        axiosInstance.put(`/docs/change-status`, {
              id: gostToReplaceId,
              status: 2
          })
      }).then(() => navigate('/'))
    }

      if(loading) return <></>
    
      return (
        <div className='container'>
          <section className={classNames('contentContainer', styles.reviewSection)}>
            <GostForm handleSubmit={addNewDocument} gost={{...getGostStub(), acceptedFirstTimeOrReplaced: `Принят взамен ${response?.primary.designation}`}}/>
          </section>
        </div>
      )
}

export default GostReplacePage