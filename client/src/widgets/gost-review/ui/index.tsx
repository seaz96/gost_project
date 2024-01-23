import React, { useState } from 'react'

import styles from './GostReview.module.scss';
import { Button } from 'shared/components';
import { gostModel } from 'entities/gost';
import { Link } from 'react-router-dom';
import classNames from 'classnames';
import { Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle } from '@mui/material';
import axios from 'axios';
import { useNavigate } from "react-router-dom"
import { useAxios } from 'shared/hooks';

interface GostReviewProps {
    gost: gostModel.Gost,
    gostId: number
}

const GostReview:React.FC<GostReviewProps> = props => {
    const {
        gost,
        gostId
    } = props
    const navigate = useNavigate()
    const {response, loading, error} = useAxios<gostModel.GostGeneralInfo[]>('https://backend-seaz96.kexogg.ru/api/docs/all-general-info')

    const [deleteModalOpen, setDeleteModalOpen] = useState(false)
    const [cancelModalOpen, setCancelModalOpen] = useState(false)

    const primaryAcceptanceDate = new Date(gost.primary.acceptanceDate)
    const primaryCommissionDate = new Date(gost.primary.commissionDate)
    const actualAcceptanceDate = gost.actual.acceptanceDate ? new Date(gost.actual.acceptanceDate) : null
    const actualCommissionDate = gost.actual.acceptanceDate ? new Date(gost.actual.commissionDate) : null

    const onDeleteSubmit = () => {
        axios.delete(`https://backend-seaz96.kexogg.ru/api/docs/delete/${gostId}`, {
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('jwt_token')}`
            }
        })
        .then(response => navigate('/'))
    }

    const cancelDoc = () => {
        axios.put(`https://backend-seaz96.kexogg.ru/api/docs/change-status`, {
            id: gostId,
            status: 1
        }, {
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('jwt_token')}`
            }
        })
        .then(response => {navigate('/')})
    }

    function getLinksById(value: {docId: number, designation: string, status: number}[]) {
        let result:string[] = []
        value?.forEach(ref => {
            result.push(ref.designation)
        })
        return result.join(', ')
    }

    return (
        <>
            <div className={styles.reviewContainer}>
                <h2 className={styles.title}>Просмотр документа</h2>
                <div className={styles.buttonsContainer}>
                    <Link onClick={() => {}} to={`/gost-edit/${gostId}`} className={classNames(styles.link, 'coloredText')}>Редактировать</Link>
                    <Button onClick={() => setDeleteModalOpen(true)} isColoredText>Удалить</Button>
                    <Button onClick={() => setCancelModalOpen(true)} isColoredText>Отменить</Button>
                    <Link to={`/gost-actualize-page/${gostId}`} className={classNames(styles.link, 'coloredText')}>Актуализировать данные</Link>
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
                            <td>{gost.actual.fullName}</td>
                        </tr>
                        <tr>
                            <td>Код ОКС</td>
                            <td>{gost.primary.codeOKS}</td>
                            <td>{gost.actual.codeOKS}</td>
                        </tr>
                        <tr>
                            <td>Сфера деятельности</td>
                            <td>{gost.primary.activityField}</td>
                            <td>{gost.actual.activityField}</td>
                        </tr>
                        <tr>
                            <td>Год принятия</td>
                            <td>
                            {
                            `${primaryAcceptanceDate.getUTCDate()}.${primaryAcceptanceDate.getUTCMonth()}.${primaryAcceptanceDate.getUTCFullYear()}`
                            }
                            </td>
                            <td>
                            { actualAcceptanceDate &&
                            `${actualAcceptanceDate.getUTCDate()}.${actualAcceptanceDate.getUTCMonth()}.${actualAcceptanceDate.getUTCFullYear()}`
                            }
                            </td>
                        </tr>
                        <tr>
                            <td>Год введения</td>
                            <td> 
                            {
                            `${primaryCommissionDate.getUTCDate()}.${primaryCommissionDate.getUTCMonth()}.${primaryCommissionDate.getUTCFullYear()}`
                            }
                            </td>
                            <td>
                            { actualCommissionDate &&
                            `${actualCommissionDate.getUTCDate()}.${actualCommissionDate.getUTCMonth()}.${actualCommissionDate.getUTCFullYear()}`
                            }
                            </td>
                        </tr>
                        <tr>
                            <td>Разработчик</td>
                            <td>{gost.primary.author}</td>
                            <td>{gost.actual.author}</td>
                        </tr>
                        <tr>
                            <td>Принят впервые/взамен</td>
                            <td>{gost.primary.acceptedFirstTimeOrReplaced}</td>
                            <td>{gost.actual.acceptedFirstTimeOrReplaced}</td>
                        </tr>
                        <tr>
                            <td>Содержание</td>
                            <td>{gost.primary.content}</td>
                            <td>{gost.actual.content}</td>
                        </tr>
                        <tr>
                            <td>Область применения</td>
                            <td>{gost.primary.applicationArea}</td>
                            <td>{gost.actual.applicationArea}</td>
                        </tr>
                        <tr>
                            <td>Ключевые слова</td>
                            <td>{gost.primary.keyWords}</td>
                            <td>{gost.actual.keyWords}</td>
                        </tr>
                        <tr>
                            <td>Ключевые фразы</td>
                            <td>{gost.primary.keyPhrases}</td>
                            <td>{gost.actual.keyPhrases}</td>
                        </tr>
                        <tr>
                            <td>Уровень принятия</td>
                            <td>{gost.primary.adoptionLevel}</td>
                            <td>{gost.actual.adoptionLevel}</td>
                        </tr>
                        <tr>
                            <td>Текст стандарта</td>
                            <td>{gost.primary.documentText}</td>
                            <td>{gost.actual.documentText}</td>
                        </tr>
                        <tr>
                            <td>Нормативные ссылки</td>
                            <td>{getLinksById(gost.references)}</td>
                            <td>{getLinksById(gost.references)}</td>
                        </tr>
                        <tr>
                            <td>Изменения</td>
                            <td>{gost.primary.changes}</td>
                            <td>{gost.actual.changes}</td>
                        </tr>
                        <tr>
                            <td>Поправки</td>
                            <td>{gost.primary.amendments}</td>
                            <td>{gost.actual.amendments}</td>
                        </tr>
                        <tr>
                            <td>Действующий/Отменён/Заменён</td>
                            <td>{gostModel.Statuses[gost.primary.status]}</td>
                            <td>{gostModel.Statuses[gost.actual.status]}</td>
                        </tr>
                        <tr>
                            <td>Уровень гармонизации</td>
                            <td>{gostModel.Harmonization[gost.primary.harmonization]}</td>
                            <td>{gostModel.Harmonization[gost.actual.harmonization]}</td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <DeleteCard 
                isOpen={deleteModalOpen}
                setIsOpen={setDeleteModalOpen}
                onSubmitFunction={onDeleteSubmit}
            />
            <CancelCard
                isOpen={cancelModalOpen}
                setIsOpen={setCancelModalOpen}
                onSubmitFunction={cancelDoc}
            />
        </>
    )
}

interface DeleteCardProps {
    isOpen: boolean,
    setIsOpen: Function,
    onSubmitFunction: Function
}

const DeleteCard:React.FC<DeleteCardProps> = props => {
    const {
        isOpen,
        setIsOpen,
        onSubmitFunction
    } = props

    return (
        <Dialog
            open={isOpen}
            onClose={() => setIsOpen(false)}
        >
            <DialogTitle id="alert-dialog-title">
                Удалить ГОСТ?
            </DialogTitle>
            <DialogContent>
                <DialogContentText id="alert-dialog-description">
                    Если вы удалите ГОСТ, он полностью удалится из базы без возможности восстановления
                </DialogContentText>
            </DialogContent>
            <DialogActions>
                <Button isColoredText onClick={() => setIsOpen(false)} className={styles.DeleteCardButton}>Отменить</Button>
                <Button isFilled onClick={() => onSubmitFunction()}  className={styles.DeleteCardButton}>
                    Удалить
                </Button>
            </DialogActions>
        </Dialog>
    )
}

interface CancelCardProps {
    isOpen: boolean,
    setIsOpen: Function,
    onSubmitFunction: Function
}

const CancelCard:React.FC<CancelCardProps> = props => {
    const {
        isOpen,
        setIsOpen,
        onSubmitFunction
    } = props

    return (
        <Dialog
            open={isOpen}
            onClose={() => setIsOpen(false)}
        >
            <DialogTitle id="alert-dialog-title">
                Отменить ГОСТ?
            </DialogTitle>
            <DialogContent>
                <DialogContentText id="alert-dialog-description">
                    Если вы Отменить ГОСТ, он перенесется в архив, а его статус поменятся на 'Отмененный'
                </DialogContentText>
            </DialogContent>
            <DialogActions>
                <Button isColoredText onClick={() => setIsOpen(false)} className={styles.DeleteCardButton}>Назад</Button>
                <Button isFilled onClick={() => onSubmitFunction()}  className={styles.DeleteCardButton}>
                    Отменить ГОСТ
                </Button>
            </DialogActions>
        </Dialog>
    )
}

export default GostReview;