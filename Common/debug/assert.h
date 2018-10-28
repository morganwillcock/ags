//=============================================================================
//
// Adventure Game Studio (AGS)
//
// Copyright (C) 1999-2011 Chris Jones and 2011-20xx others
// The full list of copyright holders can be found in the Copyright.txt
// file, which is part of this source code distribution.
//
// The AGS source code is provided under the Artistic License 2.0.
// A copy of this license can be found in the file License.txt and at
// http://www.opensource.org/licenses/artistic-license-2.0.php
//
//=============================================================================
//
// Debug assertion tools
//
//=============================================================================
#ifndef __AGS_CN_DEBUG__ASSERT_H
#define __AGS_CN_DEBUG__ASSERT_H

// TODO: revise this later (add platform-specific output maybe?)
#if defined(WINDOWS_VERSION)

    #ifdef _DEBUG
    inline void assert(bool expr)
    {
        if (!expr) {
            _asm {
                int 3
            }
        }
    }
    #else
    inline void assert(bool expr) {}
    #endif

#else // !WINDOWS_VERSION

    #ifndef _DEBUG
    #define NDEBUG
    #endif

    #include <assert.h>

#endif


#endif // __AGS_CN_DEBUG__ASSERT_H

