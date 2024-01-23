import React from 'react'
import { Filter } from 'widgets/filter';
import { GostReview } from 'widgets/gost-review';

import styles from './GostReviewsPage.module.scss'
import { useParams } from 'react-router-dom';
import { useAxios } from 'shared/hooks';
import { gostModel } from 'entities/gost';
import classNames from 'classnames';


const GostReviewPage = () => {
  const gostId = useParams().id;
  const {response, error, loading} = useAxios<gostModel.Gost>(`https://backend-seaz96.kexogg.ru/api/docs/${gostId}`)


  if(response) {
    return (
      <div className='container'>
        <section className={classNames(styles.reviewSection, 'contentContainer')}>
          <GostReview gost={response} gostId={response.docId}/>
        </section>
      </div>
    )
  } else {
    return <></>
  }
  
}

export default GostReviewPage;