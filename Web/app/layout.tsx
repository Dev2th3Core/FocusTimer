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

  title: 'Focus Timer App – Always-On-Top Desktop Timer for Windows',
  description: 'Focus Timer is a lightweight desktop productivity app that keeps your timer always on top while you work. Customize colors, place it anywhere, and stay focused without a second screen.',
  keywords: [
    'focus timer',
    'desktop timer',
    'always on top timer',
    'overlay timer',
    'productivity app',
    'windows focus app',
    'pomodoro alternative',
    'open source timer',
  ],
  authors: [{ name: 'dev2th3core', url: 'https://github.com/dev2th3core' }],
  creator: 'dev2th3core',

  // Open Graph (for social sharing on Facebook, LinkedIn, etc.)
  openGraph: {
    title: 'Focus Timer App – Always-On-Top Desktop Timer for Windows',
    description: 'A desktop focus timer that floats above your apps. Always on top, position anywhere, custom colors. Free and open source.',
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
    title: 'Focus Timer App – Always-On-Top Desktop Timer for Windows',
    description: 'A desktop focus timer that stays visible while you work. Always on top. Position anywhere. Free Windows app.',
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
