import React from 'react'

import styles from './IconButton.module.scss'
import classNames from 'classnames';

interface ButtonProps {
  children: React.ReactNode
  isFilled?: boolean
  className?: string
  onClick: Function
}

const IconButton:React.FC<ButtonProps> = props => {
  const {
    children,
    className,
    onClick,
    isFilled
  } = props

  return (
    <button 
      className={classNames(className, styles.baseButton, 
        isFilled ? styles.filledButton : '', 
      )} 
      onClick={(event) => onClick(event)}
    >
      {children}
    </button>
  )
}

export default IconButton;