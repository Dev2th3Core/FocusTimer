import React from "react"
import type { Metadata } from 'next'
import { Geist } from 'next/font/google'
import { Analytics } from '@vercel/analytics/next'
import './globals.css'

const geist = Geist({ 
  subsets: ["latin"],
  variable: '--font-sans',
});

export const metadata: Metadata = {
  // TODO: Replace with your actual domain when you deploy
  metadataBase: new URL('https://focustimer.dev2th3core.site'),

  title: 'Focus Timer - The Always-On-Top Timer for Windows',
  description: 'A lightweight, customizable timer overlay that floats above any application. Keep your focus clock directly on your screen. Free and open source.',
  keywords: ['focus timer', 'pomodoro', 'desktop timer', 'overlay timer', 'productivity app', 'windows app', 'focus app', 'open source'],
  authors: [{ name: 'dev2th3core', url: 'https://github.com/dev2th3core' }],
  creator: 'dev2th3core',

  // Open Graph (for social sharing on Facebook, LinkedIn, etc.)
  openGraph: {
    title: 'Focus Timer - The Always-On-Top Timer for Windows',
    description: 'A lightweight, customizable timer overlay that floats above any application.',
    // TODO: Replace with your actual domain
    url: 'https://focustimer.dev2th3core.site',
    siteName: 'Focus Timer',
    images: [
      {
        // NOTE: Create an 'og-image.png' (1200x630) and place it in the /public folder
        url: '/og-image.png',
        width: 1200,
        height: 630,
        alt: 'Focus Timer application shown in a laptop mockup.',
      },
    ],
    locale: 'en_US',
    type: 'website',
  },

  // Twitter Card (for sharing on X/Twitter)
  twitter: {
    card: 'summary_large_image',
    title: 'Focus Timer - The Always-On-Top Timer for Windows',
    description: 'A lightweight, customizable timer overlay that floats above any application.',
    // TODO: Add your Twitter handle if you have one
    creator: '@dev2th3core',
    images: ['/og-image.png'], // Twitter uses the same Open Graph image
  },

  // Icons
  icons: {
    icon: '/icon.ico',
    shortcut: '/icon.png',
    apple: '/apple-touch-icon.png',
  },
}

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode
}>) {
  return (
    <html lang="en">
      <body className={`${geist.variable} font-sans antialiased`}>
        {children}
        <Analytics />
      </body>
    </html>
  )
}
