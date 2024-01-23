import React from 'react'

import styles from './GostReview.module.scss';
import { Button } from 'shared/components';
import { gostModel } from 'entities/gost';

interface GostReviewProps {
    gost: gostModel.Gost
}

const GostReview:React.FC<GostReviewProps> = props => {
    const {
        gost
    } = props

    const primaryAcceptanceDate = new Date(gost.primary.acceptanceDate)
    const primaryCommissionDate = new Date(gost.primary.commissionDate)

    return (
        <div className={styles.reviewContainer}>
            <h2 className={styles.title}>Просмотр документа</h2>
            <div className={styles.buttonsContainer}>
                <Button onClick={() => {}} isColoredText>Редактироваться</Button>
                <Button onClick={() => {}} isColoredText>Архивировать</Button>
                <Button onClick={() => {}} isColoredText>Удалить</Button>
                <Button onClick={() => {}} isColoredText>Заменить</Button>
                <Button onClick={() => {}} isColoredText>Актуализировать данные</Button>
            </div>
            <table className={styles.gostTable}>
                <thead>
                    <tr>
                        <td>Поле</td>
                        <td>Первоначальное значение</td>
                        <td>Последняя актуализация</td>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>Наименование стандарта</td>
                        <td>{gost.primary.fullName}</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Код ОКС</td>
                        <td>{gost.primary.codeOKS}</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Сфера деятельности</td>
                        <td>{gost.primary.activityField}</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Год принятия</td>
                        <td>
                        {
                        `${primaryAcceptanceDate.getUTCDate()}.${primaryAcceptanceDate.getUTCMonth()}.${primaryAcceptanceDate.getUTCFullYear()}`
                        }
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Год введения</td>
                        <td> 
                        {
                        `${primaryCommissionDate.getUTCDate()}.${primaryCommissionDate.getUTCMonth()}.${primaryCommissionDate.getUTCFullYear()}`
                        }
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Разработчик</td>
                        <td>{gost.primary.author}</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Принят впервые/взамен</td>
                        <td>{gost.primary.acceptedFirstTimeOrReplaced}</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Содержание</td>
                        <td>{gost.primary.content}</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Область применения</td>
                        <td>{gost.primary.applicationArea}</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Ключевые слова</td>
                        <td>{gost.primary.keyWords}</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Ключевые фразы</td>
                        <td>{gost.primary.keyPhrases}</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Уровень принятия</td>
                        <td>{gost.primary.adoptionLevel}</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Текст стандарта</td>
                        <td>{gost.primary.documentText}</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Нормативные ссылки</td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Изменения</td>
                        <td>{gost.primary.changes}</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Поправки</td>
                        <td>{gost.primary.amendments}</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Отменен/Заменен/Действующийи</td>
                        <td>{gost.primary.status}</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Уровень гармонизации</td>
                        <td>{gost.primary.harmonization}</td>
                        <td></td>
                    </tr>
                </tbody>
            </table>
        </div>
    )
}

export default GostReview;